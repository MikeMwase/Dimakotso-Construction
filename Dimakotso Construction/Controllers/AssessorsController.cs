using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dimakotso_Construction.Data;
using Dimakotso_Construction.Models;

namespace Dimakotso_Construction.Controllers
{
    public class AssessorsController : Controller
    {
        private readonly AcademyDbContext _context;

        public AssessorsController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: Assessors
        public async Task<IActionResult> Index(string? searchTerm, string? department, string? status, int page = 1)
        {
            const int pageSize = 8;

            var allAssessors = _context.Assessors.AsQueryable();
            var totalAssessors = await allAssessors.CountAsync();
            var activeCount = await allAssessors.CountAsync(a => a.Status == "Active");
            var expiringCount = await allAssessors.CountAsync(a => a.Status == "Expiring Soon");
            var inactiveCount = await allAssessors.CountAsync(a => a.Status == "Inactive");

            var query = _context.Assessors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a =>
                    a.FirstName.Contains(searchTerm) ||
                    a.LastName.Contains(searchTerm) ||
                    a.AssessorNumber.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(a => a.Department == department);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var totalFiltered = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

            var assessors = await query
                .OrderBy(a => a.FirstName).ThenBy(a => a.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var departments = await _context.Assessors
                .Select(a => a.Department)
                .Distinct()
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .OrderBy(d => d)
                .ToListAsync();

            var viewModel = new AssessorIndexViewModel
            {
                Assessors = assessors,
                SearchTerm = searchTerm,
                Department = department,
                Status = status,
                Departments = departments,
                PageIndex = page,
                TotalPages = totalPages,
                TotalFilteredAssessors = totalFiltered,
                TotalAssessors = totalAssessors,
                ActiveCount = activeCount,
                ExpiringCount = expiringCount,
                InactiveCount = inactiveCount
            };

            return View(viewModel);
        }

        // GET: Assessors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var assessors = await _context.Assessors.FirstOrDefaultAsync(m => m.Id == id);
            if (assessors == null) return NotFound();

            return View(assessors);
        }

        private void PopulateCoursePicker(Assessors? assessor = null)
        {
            ViewBag.Courses = _context.Courses
                .Where(c => c.Status == CourseStatus.Active)
                .OrderBy(c => c.CourseName)
                .ToList();

            ViewBag.SelectedAccreditations = assessor?.AssessorCourses?
                .ToDictionary(ac => ac.CourseId, ac => ac.AccreditationNumber)
                ?? new Dictionary<int, string>();

            ViewBag.KnownQualificationNames = _context.AssessorQualifications
                .Select(q => q.QualificationName)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }

        // GET: Assessors/Create
        public IActionResult Create()
        {
            PopulateCoursePicker();
            return View();
        }

        // POST: Assessors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Id,FirstName,LastName,Contact,AssessorNumber,Department,Status,qualifications")] Assessors assessors,
    List<int>? selectedCourseIds,
    Dictionary<int, string>? accreditationNumbers,
    List<QualificationUploadInput>? qualificationUploads)
        {
            if (!ModelState.IsValid)
            {
                PopulateCoursePicker(assessors);
                return View(assessors);
            }

            try
            {
                if (selectedCourseIds != null)
                {
                    foreach (var courseId in selectedCourseIds)
                    {
                        string accNumber = string.Empty;
                        if (accreditationNumbers != null && accreditationNumbers.TryGetValue(courseId, out var val))
                        {
                            accNumber = val ?? string.Empty;
                        }

                        assessors.AssessorCourses.Add(new AssessorCourse
                        {
                            CourseId = courseId,
                            AccreditationNumber = accNumber
                        });
                    }
                }

                if (qualificationUploads != null && qualificationUploads.Any())
                {
                    foreach (var input in qualificationUploads)
                    {
                        if (input.File != null && input.File.Length > 0 && !string.IsNullOrWhiteSpace(input.QualificationName))
                        {
                            var extension = Path.GetExtension(input.File.FileName).ToLowerInvariant();
                            if (AllowedExtensions.Contains(extension) && input.File.Length <= MaxFileSizeBytes)
                            {
                                using var memoryStream = new MemoryStream();
                                await input.File.CopyToAsync(memoryStream);

                                assessors.QualificationDocuments.Add(new AssessorQualification
                                {
                                    QualificationName = input.QualificationName,
                                    IssuingBody = input.IssuingBody,
                                    DateObtained = input.DateObtained,
                                    ExpiryDate = input.ExpiryDate,
                                    FileName = input.File.FileName,
                                    ContentType = input.File.ContentType,
                                    FileSizeBytes = input.File.Length,
                                    FileData = memoryStream.ToArray(),
                                    UploadedOn = DateTime.UtcNow,
                                    IsActive = true
                                });
                            }
                        }
                    }
                }

                var activeQualificationCount = assessors.QualificationDocuments
                .Count(d => d.ExpiryDate == null || d.ExpiryDate > DateTime.UtcNow);

                assessors.qualifications = activeQualificationCount > 0
                    ? $"{activeQualificationCount} Active"
                    : "0 Active";

                _context.Add(assessors);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.InnerException?.Message ?? ex.Message;

                PopulateCoursePicker(assessors);
                return View(assessors);
            }
        }

        // GET: Assessors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var assessors = await _context.Assessors
                .Include(a => a.AssessorCourses)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessors == null) return NotFound();

            PopulateCoursePicker(assessors);
            return View(assessors);
        }

        // POST: Assessors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,FirstName,LastName,Contact,AssessorNumber,Department,Status,qualifications")] Assessors assessors,
            List<int>? selectedCourseIds,
            Dictionary<int, string>? accreditationNumbers)
        {
            if (id != assessors.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateCoursePicker(assessors);
                return View(assessors);
            }

            var existingAssessor = await _context.Assessors
                .Include(a => a.AssessorCourses)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAssessor == null) return NotFound();

            existingAssessor.FirstName = assessors.FirstName;
            existingAssessor.LastName = assessors.LastName;
            existingAssessor.Contact = assessors.Contact;
            existingAssessor.AssessorNumber = assessors.AssessorNumber;
            existingAssessor.Department = assessors.Department;
            existingAssessor.Status = assessors.Status;
            existingAssessor.qualifications = assessors.qualifications;

            selectedCourseIds ??= new List<int>();

            var toRemove = existingAssessor.AssessorCourses
                .Where(ac => !selectedCourseIds.Contains(ac.CourseId))
                .ToList();

            foreach (var ac in toRemove)
            {
                existingAssessor.AssessorCourses.Remove(ac);
            }

            foreach (var courseId in selectedCourseIds)
            {
                string accNumber = string.Empty;
                if (accreditationNumbers != null && accreditationNumbers.TryGetValue(courseId, out var val))
                {
                    accNumber = val ?? string.Empty;
                }

                var existing = existingAssessor.AssessorCourses.FirstOrDefault(ac => ac.CourseId == courseId);

                if (existing != null)
                {
                    existing.AccreditationNumber = accNumber;
                }
                else
                {
                    existingAssessor.AssessorCourses.Add(new AssessorCourse
                    {
                        AssessorId = existingAssessor.Id,
                        CourseId = courseId,
                        AccreditationNumber = accNumber
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessorsExists(assessors.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Assessors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var assessors = await _context.Assessors.FirstOrDefaultAsync(m => m.Id == id);
            if (assessors == null) return NotFound();

            return View(assessors);
        }

        // POST: Assessors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessors = await _context.Assessors.FindAsync(id);
            if (assessors != null)
            {
                _context.Assessors.Remove(assessors);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };

        // GET: Assessors/Qualifications/5
        public async Task<IActionResult> Qualifications(int id)
        {
            var assessor = await _context.Assessors
                .Include(a => a.QualificationDocuments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessor == null) return NotFound();

            return View(assessor);
        }

        // POST: Assessors/UploadQualification
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(MaxFileSizeBytes)]
        public async Task<IActionResult> UploadQualification(
            int assessorId,
            string qualificationName,
            string? issuingBody,
            DateTime? dateObtained,
            DateTime? expiryDate,
            IFormFile file)
        {
            var assessor = await _context.Assessors.FindAsync(assessorId);
            if (assessor == null) return NotFound();

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
            }
            else
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Only PDF, JPG, and PNG files are allowed.");
                }
                else if (file.Length > MaxFileSizeBytes)
                {
                    ModelState.AddModelError("", "File exceeds the 10 MB size limit.");
                }
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Qualifications), new { id = assessorId });
            }

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var qualification = new AssessorQualification
            {
                AssessorId = assessorId,
                QualificationName = qualificationName,
                IssuingBody = issuingBody,
                DateObtained = dateObtained,
                ExpiryDate = expiryDate,
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileSizeBytes = file.Length,
                FileData = fileBytes,
                UploadedOn = DateTime.UtcNow
            };

            _context.AssessorQualifications.Add(qualification);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Qualifications), new { id = assessorId });
        }

        // GET: Assessors/DownloadQualification/5
        public async Task<IActionResult> DownloadQualification(int id)
        {
            var qualification = await _context.AssessorQualifications.FindAsync(id);
            if (qualification == null) return NotFound();

            return File(qualification.FileData, qualification.ContentType, qualification.FileName);
        }

        // POST: Assessors/DeleteQualification/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQualification(int id, int assessorId)
        {
            var qualification = await _context.AssessorQualifications.FindAsync(id);
            if (qualification != null)
            {
                _context.AssessorQualifications.Remove(qualification);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Qualifications), new { id = assessorId });
        }

        // GET: Assessors/GetByCourse?courseId=5
        [HttpGet]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var assessors = await _context.AssessorCourses
                .Where(ac => ac.CourseId == courseId && ac.Assessor.Status == "Active")
                .Select(ac => new
                {
                    id = ac.Assessor.Id,
                    name = $"{ac.Assessor.FirstName} {ac.Assessor.LastName} ({ac.Assessor.AssessorNumber})",
                    accreditationNumber = ac.AccreditationNumber,
                    qualifications = ac.Assessor.qualifications
                })
                .ToListAsync();

            return Json(assessors);
        }

        private bool AssessorsExists(int id)
        {
            return _context.Assessors.Any(e => e.Id == id);
        }
    }
}