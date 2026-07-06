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
    public class StudentEnrollmentsController : Controller
    {
        private readonly AcademyDbContext _context;

        public StudentEnrollmentsController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: StudentEnrollments
        public async Task<IActionResult> Index()
        {
            var academyDbContext = _context.StudentEnrollments.Include(s => s.WorkplacePlacement);
            return View(await academyDbContext.ToListAsync());
        }

        // GET: StudentEnrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEnrollment = await _context.StudentEnrollments
                .Include(s => s.WorkplacePlacement)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentEnrollment == null)
            {
                return NotFound();
            }

            return View(studentEnrollment);
        }

        // GET: StudentEnrollments/Create
        public IActionResult Create()
        {
            ViewData["WorkplacePlacementId"] = new SelectList(_context.WorkplacePlacements, "Id", "CompanyVatNumber");
            return View();
        }

        // POST: StudentEnrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegistrationNumber,FirstNames,Surname,IdentificationNumber,DateOfBirth,Gender,Equity,Citizenship,DisabilityStatus,CurrentEmployment,HighestQualification,Email,MobileNumber,HomeAddress,PostalCode,TargetProgramTitle,SaqaId,Status,DateCreated,HasConsentedToPopiaDataSharing,WorkplacePlacementId")] StudentEnrollment studentEnrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentEnrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkplacePlacementId"] = new SelectList(_context.WorkplacePlacements, "Id", "CompanyVatNumber", studentEnrollment.WorkplacePlacementId);
            return View(studentEnrollment);
        }

        // GET: StudentEnrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEnrollment = await _context.StudentEnrollments.FindAsync(id);
            if (studentEnrollment == null)
            {
                return NotFound();
            }
            ViewData["WorkplacePlacementId"] = new SelectList(_context.WorkplacePlacements, "Id", "CompanyVatNumber", studentEnrollment.WorkplacePlacementId);
            return View(studentEnrollment);
        }

        // POST: StudentEnrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,FirstNames,Surname,IdentificationNumber,DateOfBirth,Gender,Equity,Citizenship,DisabilityStatus,CurrentEmployment,HighestQualification,Email,MobileNumber,HomeAddress,PostalCode,TargetProgramTitle,SaqaId,Status,DateCreated,HasConsentedToPopiaDataSharing,WorkplacePlacementId")] StudentEnrollment studentEnrollment)
        {
            if (id != studentEnrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentEnrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentEnrollmentExists(studentEnrollment.Id))
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
            ViewData["WorkplacePlacementId"] = new SelectList(_context.WorkplacePlacements, "Id", "CompanyVatNumber", studentEnrollment.WorkplacePlacementId);
            return View(studentEnrollment);
        }

        // GET: StudentEnrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentEnrollment = await _context.StudentEnrollments
                .Include(s => s.WorkplacePlacement)
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
