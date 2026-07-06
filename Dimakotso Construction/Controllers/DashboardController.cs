using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Dimakotso_Construction.Data;
using Dimakotso_Construction.Models;
using Dimakotso_Construction.Models.Enums;
using System.Threading.Tasks;

namespace Dimakotso_Construction.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AcademyDbContext _context;

        public DashboardController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var totalStudents = await _context.StudentEnrollments.CountAsync();

            var activeTraining = await _context.StudentEnrollments
                .CountAsync(s => s.Status == EnrollmentStatus.ActiveTraining);

            var pendingVerification = await _context.StudentEnrollments
                .CountAsync(s => s.Status == EnrollmentStatus.Registered);

            ViewBag.TotalStudents = totalStudents;
            ViewBag.ActiveTraining = activeTraining;
            ViewBag.PendingVerification = pendingVerification;

            var records = await _context.StudentEnrollments
                .Include(s => s.WorkplacePlacement)
                .OrderByDescending(s => s.DateCreated)
                .ToListAsync();

            return View(records);
        }
    }
}