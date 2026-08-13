using Dimakotso_Construction.Data;
using Dimakotso_Construction.Models;
using Dimakotso_Construction.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dimakotso_Construction.Controllers
{
    public class StudentEnrollmentsController : Controller
    {
        private readonly AcademyDbContext _context;

        public StudentEnrollmentsController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: StudentEnrollments
        // GET: StudentEnrollments
        public async Task<IActionResult> Index(string? searchTerm, EnrollmentStatus? status, int? employerId, int page = 1)
        {
            const int pageSize = 9;

            var allStudents = _context.StudentEnrollments.AsQueryable();
            var totalStudents = await allStudents.CountAsync();
            var activeTrainingCount = await allStudents.CountAsync(s => s.Status == EnrollmentStatus.ActiveTraining);
            var registeredCount = await allStudents.CountAsync(s => s.Status == EnrollmentStatus.Registered);
            var otherStatusCount = totalStudents - activeTrainingCount - registeredCount;

            var query = _context.StudentEnrollments.Include(s => s.Employer).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    s.FirstNames.Contains(searchTerm) ||
                    s.Surname.Contains(searchTerm) ||
                    s.RegistrationNumber.Contains(searchTerm) ||
                    s.IdentificationNumber.Contains(searchTerm) ||
                    s.Email.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            if (employerId.HasValue)
            {
                query = query.Where(s => s.EmployerId == employerId.Value);
            }

            var totalFiltered = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

            var students = await query
                 .OrderByDescending(s => s.DateCreated)
                 .ThenByDescending(s => s.Id)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

            var employers = await _context.Employers
                .OrderBy(e => e.CompanyName)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.CompanyName,
                    Selected = e.Id == employerId
                })
                .ToListAsync();

            var viewModel = new StudentEnrollmentIndexViewModel
            {
                Students = students,
                SearchTerm = searchTerm,
                Status = status,
                EmployerId = employerId,
                Employers = employers,
                PageIndex = page,
                TotalPages = totalPages,
                TotalFilteredStudents = totalFiltered,
                TotalStudents = totalStudents,
                ActiveTrainingCount = activeTrainingCount,
                RegisteredCount = registeredCount,
                OtherStatusCount = otherStatusCount
            };

            return View(viewModel);
        }

        // GET: StudentEnrollments/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var studentEnrollment = await _context.StudentEnrollments
        //        .Include(s => s.Employer)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (studentEnrollment == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(studentEnrollment);
        //}

        // GET: StudentEnrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var studentEnrollment = await _context.StudentEnrollments
                .Include(s => s.Employer)
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studentEnrollment == null) return NotFound();

            return View(studentEnrollment);
        }

        private void PopulateDropdowns(StudentEnrollment? student = null)
        {
            var employers = _context.Employers
                            .OrderBy(e => e.CompanyName)
                            .ToList();

            ViewBag.TestCount = employers.Count;

            ViewBag.EmployerId = new SelectList(
                _context.Employers.OrderBy(c => c.CompanyName),
                "Id", "CompanyName", student?.EmployerId);

            ViewBag.Gender = new SelectList(new[] { "Male", "Female", "Other" }, student?.Gender);
            ViewBag.Equity = new SelectList(Enum.GetValues(typeof(EquityCode)), student?.Equity);
            ViewBag.Citizenship = new SelectList(Enum.GetValues(typeof(CitizenStatusCode)), student?.Citizenship);
            ViewBag.DisabilityStatus = new SelectList(Enum.GetValues(typeof(DisabilityCode)), student?.DisabilityStatus);
            ViewBag.CurrentEmployment = new SelectList(Enum.GetValues(typeof(EmploymentStatus)), student?.CurrentEmployment);
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(EnrollmentStatus)), student?.Status);

            // Course picker
            ViewBag.Courses = _context.Courses
                .Where(c => c.Status == CourseStatus.Active)
                .OrderBy(c => c.CourseName)
                .ToList();

            ViewBag.SelectedCourseIds = student?.StudentCourses?.Select(sc => sc.CourseId).ToList()
                ?? new List<int>();
        }

        // GET: StudentEnrollments/Create
        public IActionResult Create()
        {
            var studentEnrollment = new StudentEnrollment
            {
                RegistrationNumber = $"DC-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                DateCreated = DateTime.Today,
                Status = EnrollmentStatus.Registered
            };

            PopulateDropdowns(studentEnrollment);

            return View(studentEnrollment);
        }

        // POST: StudentEnrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,RegistrationNumber,FirstNames,Surname,IdentificationNumber,DateOfBirth,Gender,Equity,Citizenship,DisabilityStatus,CurrentEmployment,HighestQualification,Email,MobileNumber,HomeAddress,City,PostalCode,Status,DateCreated,HasConsentedToPopiaDataSharing,EmployerId")] StudentEnrollment studentEnrollment,
            List<int>? selectedCourseIds)
        {
            if (ModelState.IsValid)
            {
                studentEnrollment.RegistrationNumber = $"DC-{Guid.NewGuid().ToString()[..8].ToUpper()}";
                studentEnrollment.DateCreated = DateTime.Today;

                if (selectedCourseIds != null)
                {
                    foreach (var courseId in selectedCourseIds)
                    {
                        studentEnrollment.StudentCourses.Add(new StudentCourse
                        {
                            CourseId = courseId,
                            EnrolmentDate = DateTime.UtcNow,
                            Status = EnrollmentStatus.Registered
                        });
                    }
                }

                _context.StudentEnrollments.Add(studentEnrollment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(studentEnrollment);
            return View(studentEnrollment);
        }

        // GET: StudentEnrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var studentEnrollment = await _context.StudentEnrollments
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studentEnrollment == null) return NotFound();

            PopulateDropdowns(studentEnrollment);
            return View(studentEnrollment);
        }

        // POST: StudentEnrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,FirstNames,Surname,IdentificationNumber,DateOfBirth,Gender,Equity,Citizenship,DisabilityStatus,CurrentEmployment,HighestQualification,Email,MobileNumber,HomeAddress,City,PostalCode,Status,HasConsentedToPopiaDataSharing,EmployerId")] StudentEnrollment studentEnrollment,
            List<int>? selectedCourseIds)
        {
            if (id != studentEnrollment.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(studentEnrollment);
                return View(studentEnrollment);
            }

            var existingStudent = await _context.StudentEnrollments
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingStudent == null) return NotFound();

            // Update editable fields
            existingStudent.FirstNames = studentEnrollment.FirstNames;
            existingStudent.Surname = studentEnrollment.Surname;
            existingStudent.IdentificationNumber = studentEnrollment.IdentificationNumber;
            existingStudent.DateOfBirth = studentEnrollment.DateOfBirth;
            existingStudent.Gender = studentEnrollment.Gender;
            existingStudent.Equity = studentEnrollment.Equity;
            existingStudent.Citizenship = studentEnrollment.Citizenship;
            existingStudent.DisabilityStatus = studentEnrollment.DisabilityStatus;
            existingStudent.CurrentEmployment = studentEnrollment.CurrentEmployment;
            existingStudent.HighestQualification = studentEnrollment.HighestQualification;
            existingStudent.Email = studentEnrollment.Email;
            existingStudent.MobileNumber = studentEnrollment.MobileNumber;
            existingStudent.HomeAddress = studentEnrollment.HomeAddress;
            existingStudent.City = studentEnrollment.City;
            existingStudent.PostalCode = studentEnrollment.PostalCode;
            existingStudent.Status = studentEnrollment.Status;
            existingStudent.HasConsentedToPopiaDataSharing = studentEnrollment.HasConsentedToPopiaDataSharing;
            existingStudent.EmployerId = studentEnrollment.EmployerId;

            // Sync course enrollments
            selectedCourseIds ??= new List<int>();
            var currentCourseIds = existingStudent.StudentCourses.Select(sc => sc.CourseId).ToList();

            var toRemove = existingStudent.StudentCourses
                .Where(sc => !selectedCourseIds.Contains(sc.CourseId))
                .ToList();
            foreach (var sc in toRemove)
            {
                existingStudent.StudentCourses.Remove(sc);
            }

            var toAdd = selectedCourseIds.Except(currentCourseIds);
            foreach (var courseId in toAdd)
            {
                existingStudent.StudentCourses.Add(new StudentCourse
                {
                    StudentEnrollmentId = existingStudent.Id,
                    CourseId = courseId,
                    EnrolmentDate = DateTime.UtcNow,
                    Status = EnrollmentStatus.Registered
                });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentEnrollmentExists(id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: StudentEnrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEnrollment = await _context.StudentEnrollments
                .Include(s => s.Employer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentEnrollment == null)
            {
                return NotFound();
            }

            return View(studentEnrollment);
        }

        // POST: StudentEnrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentEnrollment = await _context.StudentEnrollments.FindAsync(id);
            if (studentEnrollment != null)
            {
                _context.StudentEnrollments.Remove(studentEnrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentEnrollmentExists(int id)
        {
            return _context.StudentEnrollments.Any(e => e.Id == id);
        }
    }
}
