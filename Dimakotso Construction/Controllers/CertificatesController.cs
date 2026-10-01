using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dimakotso_Construction.Data;
using Dimakotso_Construction.Models;

namespace Dimakotso_Construction.Controllers
{
    public class CertificatesController : Controller
    {
        private readonly AcademyDbContext _context;

        public CertificatesController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: Certificates
        public async Task<IActionResult> Index(string? searchTerm, CertificateTrainingType? type, CertificateStatus? status, int? courseId, int page = 1)
        {
            const int pageSize = 9;
            var expiryThreshold = DateTime.UtcNow.AddDays(30);

            var allCertificates = _context.Certificates.AsQueryable();
            var totalCertificates = await allCertificates.CountAsync();
            var activeCount = await allCertificates.CountAsync(c => c.Status == CertificateStatus.Active);
            var expiringSoonCount = await allCertificates.CountAsync(c =>
                !c.NoExpiryAdviseRenew &&
                c.ExpiryDate != null &&
                c.ExpiryDate <= expiryThreshold &&
                c.ExpiryDate >= DateTime.UtcNow);
            var expiredCount = await allCertificates.CountAsync(c => c.Status == CertificateStatus.Expired);

            var query = _context.Certificates
                .Include(c => c.Assessor)
                .Include(c => c.Course)
                .Include(c => c.StudentEnrollment)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    (c.CertificateNumber != null && c.CertificateNumber.Contains(searchTerm)) ||
                    c.JobNumber.Contains(searchTerm) ||
                    (c.CertificateTitle != null && c.CertificateTitle.Contains(searchTerm)) ||
                    c.StudentEnrollment.FirstNames.Contains(searchTerm) ||
                    c.StudentEnrollment.Surname.Contains(searchTerm) ||
                    c.Course.CourseCode.Contains(searchTerm));
            }

            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (courseId.HasValue)
            {
                query = query.Where(c => c.CourseId == courseId.Value);
            }

            var totalFiltered = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

            var certificates = await query
                .OrderByDescending(c => c.IssueDate)
                .ThenByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var courses = await _context.Courses
                .OrderBy(c => c.CourseName)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CourseCode + " — " + c.CourseName,
                    Selected = c.Id == courseId
                })
                .ToListAsync();

            var viewModel = new CertificateIndexViewModel
            {
                Certificates = certificates,
                SearchTerm = searchTerm,
                Type = type,
                Status = status,
                CourseId = courseId,
                Courses = courses,
                PageIndex = page,
                TotalPages = totalPages,
                TotalFilteredCertificates = totalFiltered,
                TotalCertificates = totalCertificates,
                ActiveCount = activeCount,
                ExpiringSoonCount = expiringSoonCount,
                ExpiredCount = expiredCount
            };

            return View(viewModel);
        }

        // GET: Certificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .Include(c => c.Assessor)
                .Include(c => c.Course)
                .Include(c => c.StudentEnrollment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // GET: Certificates/Create
        public async Task<IActionResult> Create(int? studentEnrollmentId)
        {
            var viewModel = new CertificateCreateViewModel();

            if (!studentEnrollmentId.HasValue)
            {
                viewModel.RequiresStudentSelection = true;
                viewModel.Students = await _context.StudentEnrollments
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.FirstNames + " " + s.Surname })
                    .ToListAsync();
                return View(viewModel);
            }

            var student = await _context.StudentEnrollments
                .Include(s => s.Employer)
                .FirstOrDefaultAsync(s => s.Id == studentEnrollmentId.Value);

            if (student == null)
            {
                viewModel.RequiresStudentSelection = true;
                viewModel.ErrorMessage = "That student could not be found. Please select a student from the enrollment list.";
                viewModel.Students = await _context.StudentEnrollments
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.FirstNames + " " + s.Surname })
                    .ToListAsync();
                return View(viewModel);
            }

            viewModel.Certificate.StudentEnrollmentId = student.Id;
            viewModel.LearnerFirstName = student.FirstNames;
            viewModel.LearnerMiddleNames = student.MiddleNames;
            viewModel.LearnerSurname = student.Surname;
            viewModel.LearnerEmail = student.Email;
            viewModel.LearnerMobileNumber = student.MobileNumber;
            viewModel.LearnerHomeAddress = student.HomeAddress;
            viewModel.LearnerCity = student.City;
            viewModel.LearnerPostalCode = student.PostalCode;
            viewModel.LearnerIdentificationNumber = student.IdentificationNumber;

            if (student.Employer != null)
            {
                viewModel.HasEmployer = true;
                viewModel.EmployerCompanyName = student.Employer.CompanyName;
                viewModel.EmployerContactPerson = student.Employer.ContactPerson;
                viewModel.EmployerContactEmail = student.Employer.ContactEmail;
                viewModel.EmployerContactPhone = student.Employer.ContactPhone;
                viewModel.EmployerCity = student.Employer.City;
                viewModel.EmployerProvince = student.Employer.Province;
            }
            else
            {
                viewModel.HasEmployer = false;
            }

            var enrolledCourseIds = await _context.StudentCourses
                .Where(sc => sc.StudentEnrollmentId == studentEnrollmentId.Value)
                .Select(sc => sc.CourseId)
                .ToListAsync();

            if (!enrolledCourseIds.Any())
            {
                viewModel.ErrorMessage = $"{student.FirstNames} {student.Surname} is not enrolled in any courses yet. Enroll them in a course before issuing a certificate.";
                return View(viewModel);
            }

            var enrolledCourses = await _context.Courses
                .Where(c => enrolledCourseIds.Contains(c.Id))
                .ToListAsync();

            viewModel.CourseInfoJson = System.Text.Json.JsonSerializer.Serialize(
                enrolledCourses.Select(c => new
                {
                    c.Id,
                    c.CourseName,
                    c.CourseCode,
                    c.NQFLevel,
                    c.Credits,
                    c.DurationHours,
                    c.Description,
                    c.UnitStandardNumber,
                    Certification = c.Certification.ToString()
                }));

            viewModel.Courses = enrolledCourses
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CourseCode + " - " + c.CourseName })
                .ToList();

            if (enrolledCourses.Count == 1)
            {
                var only = enrolledCourses[0];

                viewModel.Certificate.CourseId = only.Id;
                viewModel.Certificate.CertificateTitle = only.CourseName;
                viewModel.Certificate.CertificateLevel = only.NQFLevel > 0 ? $"NQF Level {only.NQFLevel}" : null;
                viewModel.Certificate.CertificateCategory = only.Certification.ToString();
                viewModel.Certificate.CertificateDescription = !string.IsNullOrWhiteSpace(only.Description)
                    ? only.Description
                    : $"{only.CourseCode} – {only.CourseName}";

                // Populate the Course Information panel directly — no AJAX round trip needed
                viewModel.SelectedCourseName = only.CourseName;
                viewModel.SelectedCourseCode = only.CourseCode;
                viewModel.SelectedCourseNqfLevel = only.NQFLevel;
                viewModel.SelectedCourseDescription = only.Description;
                viewModel.SelectedCourseCredits = only.Credits;
                viewModel.SelectedCourseDurationHours = only.DurationHours;
                viewModel.SelectedCourseUnitStandardNumber = only.UnitStandardNumber;

                viewModel.Assessors = await _context.Assessors
                    .Where(a => a.Status == "Active" && a.AssessorCourses.Any(ac => ac.CourseId == only.Id))
                    .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName + " " + a.LastName })
                    .ToListAsync();
            }

            return View(viewModel);
        }

        // POST: Certificates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateCreateViewModel viewModel)
        {
            var certificate = viewModel.Certificate;

            var student = certificate.StudentEnrollmentId > 0
                ? await _context.StudentEnrollments
                    .Include(s => s.Employer)
                    .FirstOrDefaultAsync(s => s.Id == certificate.StudentEnrollmentId)
                : null;

            var enrolledCourseIds = student != null
                ? await _context.StudentCourses
                    .Where(sc => sc.StudentEnrollmentId == certificate.StudentEnrollmentId)
                    .Select(sc => sc.CourseId)
                    .ToListAsync()
                : new List<int>();

            if (student == null)
            {
                ModelState.AddModelError("", "Please select a student before issuing a certificate.");
            }
            else if (!enrolledCourseIds.Any())
            {
                ModelState.AddModelError("",
                    $"{student.FirstNames} {student.Surname} is not enrolled in any courses.");
            }

            ModelState.Remove("Certificate.StudentEnrollment");
            ModelState.Remove("Certificate.Course");
            ModelState.Remove("Certificate.Assessor");

            if (ModelState.IsValid)
            {
                _context.Add(certificate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.RequiresStudentSelection = student == null;

            if (student != null)
            {
                viewModel.LearnerFirstName = student.FirstNames;
                viewModel.LearnerMiddleNames = student.MiddleNames;
                viewModel.LearnerSurname = student.Surname;
                viewModel.LearnerEmail = student.Email;
                viewModel.LearnerMobileNumber = student.MobileNumber;
                viewModel.LearnerHomeAddress = student.HomeAddress;
                viewModel.LearnerCity = student.City;
                viewModel.LearnerPostalCode = student.PostalCode;
                viewModel.LearnerIdentificationNumber = student.IdentificationNumber;

                viewModel.HasEmployer = student.Employer != null;
                if (student.Employer != null)
                {
                    viewModel.EmployerCompanyName = student.Employer.CompanyName;
                    viewModel.EmployerContactPerson = student.Employer.ContactPerson;
                    viewModel.EmployerContactEmail = student.Employer.ContactEmail;
                    viewModel.EmployerContactPhone = student.Employer.ContactPhone;
                    viewModel.EmployerCity = student.Employer.City;
                    viewModel.EmployerProvince = student.Employer.Province;
                }

                viewModel.Courses = await _context.Courses
                    .Where(c => enrolledCourseIds.Contains(c.Id))
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CourseCode })
                    .ToListAsync();

                viewModel.Assessors = await _context.Assessors
                   .Where(a => a.Status == "Active" && a.AssessorCourses.Any(ac => ac.CourseId == certificate.CourseId))
                   .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FirstName + " " + a.LastName })
                   .ToListAsync();
            }
            else
            {
                viewModel.Students = await _context.StudentEnrollments
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.FirstNames + " " + s.Surname })
                    .ToListAsync();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoursesByStudent(int studentEnrollmentId)
        {
            var courses = await _context.StudentCourses
                .Where(sc => sc.StudentEnrollmentId == studentEnrollmentId)
                .Select(sc => new
                {
                    id = sc.Course.Id,
                    code = sc.Course.CourseCode,
                    name = sc.Course.CourseName
                })
                .ToListAsync();

            return Json(courses);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseDetails(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return NotFound();
            return Json(new
            {
                courseCode = course.CourseCode,
                courseName = course.CourseName,
                description = course.Description,
                nqfLevel = course.NQFLevel,
                credits = course.Credits,
                durationHours = course.DurationHours,
                unitStandardNumber = course.UnitStandardNumber,
                certification = course.Certification.ToString()
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAssessorsByCourse(int courseId)
        {
            var assessors = await _context.AssessorCourses
                .Where(ac => ac.CourseId == courseId && ac.Assessor.Status == "Active")
                .Select(ac => new
                {
                    id = ac.Assessor.Id,
                    name = $"{ac.Assessor.FirstName} {ac.Assessor.LastName} ({ac.Assessor.AssessorNumber})",
                    qualification = ac.Assessor.qualifications,
                    assessorNumber = ac.Assessor.AssessorNumber,
                    department = ac.Assessor.Department,
                    contact = ac.Assessor.Contact,
                    accreditationNumber = ac.AccreditationNumber ?? "Not recorded",
                    certificationBody = ac.Course.Certification.ToString()
                })
                .ToListAsync();

            return Json(assessors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssessorDetails(int assessorId, int? courseId)
        {
            var assessor = await _context.Assessors.FindAsync(assessorId);
            if (assessor == null) return NotFound();

            string? accreditationNumber = null;
            string? certificationBody = null;

            if (courseId.HasValue)
            {
                var link = await _context.Set<AssessorCourse>()
                    .FirstOrDefaultAsync(ac => ac.AssessorId == assessorId && ac.CourseId == courseId.Value);
                accreditationNumber = link?.AccreditationNumber;

                var course = await _context.Courses.FindAsync(courseId.Value);
                certificationBody = course?.Certification.ToString();
            }

            return Json(new
            {
                assessorNumber = assessor.AssessorNumber,
                department = assessor.Department,
                contact = assessor.Contact,
                qualification = assessor.qualifications,
                accreditationNumber = accreditationNumber ?? "Not recorded",
                certificationBody = certificationBody ?? "N/A"
            });
        }

        

        // GET: Certificates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }
            ViewData["AssessorId"] = new SelectList(_context.Assessors, "Id", "Id", certificate.AssessorId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseCode", certificate.CourseId);
            ViewData["StudentEnrollmentId"] = new SelectList(_context.StudentEnrollments, "Id", "Email", certificate.StudentEnrollmentId);
            return View(certificate);
        }

        // POST: Certificates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CertificateNumber,JobNumber,Type,CertificateCategory,Status,CertificateTitle,CertificateLevel,CertificateDescription,StudentEnrollmentId,CourseId,AssessorId,SelectedAssessorQualification,MachineCode,MachineDescription,MachineCapacity,MachineModel,NeedsFitnessCertificate,NoExpiryAdviseRenew,IssueDate,ExpiryDate,TheoreticalPercentage,PracticalPercentage,FinalPercentage,AssessmentMetadata,VerificationAndCompliance,Notes")] Certificate certificate)
        {
            if (id != certificate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(certificate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificateExists(certificate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssessorId"] = new SelectList(_context.Assessors, "Id", "Id", certificate.AssessorId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseCode", certificate.CourseId);
            ViewData["StudentEnrollmentId"] = new SelectList(_context.StudentEnrollments, "Id", "Email", certificate.StudentEnrollmentId);
            return View(certificate);
        }

        // GET: Certificates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .Include(c => c.Assessor)
                .Include(c => c.Course)
                .Include(c => c.StudentEnrollment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CertificateExists(int id)
        {
            return _context.Certificates.Any(e => e.Id == id);
        }
    }
}