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
    public class AssessorsController : Controller
    {
        private readonly AcademyDbContext _context;

        public AssessorsController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: Assessors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Assessors.ToListAsync());
        }

        // GET: Assessors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessors = await _context.Assessors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessors == null)
            {
                return NotFound();
            }

            return View(assessors);
        }

        // GET: Assessors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assessors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Contact,EmployeeId,Department,Status,qualifications")] Assessors assessors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assessors);
        }

        // GET: Assessors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessors = await _context.Assessors.FindAsync(id);
            if (assessors == null)
            {
                return NotFound();
            }
            return View(assessors);
        }

        // POST: Assessors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Contact,EmployeeId,Department,Status,qualifications")] Assessors assessors)
        {
            if (id != assessors.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessorsExists(assessors.Id))
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
            return View(assessors);
        }

        // GET: Assessors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessors = await _context.Assessors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessors == null)
            {
                return NotFound();
            }

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

        private bool AssessorsExists(int id)
        {
            return _context.Assessors.Any(e => e.Id == id);
        }
    }
}
