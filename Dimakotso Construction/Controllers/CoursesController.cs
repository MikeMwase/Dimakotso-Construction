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
    public class CoursesController : Controller
    {
        private readonly AcademyDbContext _context;

        public CoursesController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
        {
            const int pageSize = 6;

            // Full dataset stats — independent of search/paging
            var allCourses = _context.Courses.AsQueryable();
            var totalCourses = await allCourses.CountAsync();
            var activeCourses = await allCourses.CountAsync(c => c.Status == CourseStatus.Active);
            var totalEnrolled = await _context.Set<StudentCourse>().CountAsync();
            var totalHours = await allCourses.SumAsync(c => c.DurationHours);

            // Filtered query
            var query = _context.Courses.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.CourseName.Contains(searchTerm) ||
                    c.CourseCode.Contains(searchTerm));
            }

            var totalFiltered = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

            var courses = await query
                .OrderBy(c => c.CourseName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new CourseIndexViewModel
            {
                Courses = courses,
                SearchTerm = searchTerm,
                PageIndex = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalFilteredCourses = totalFiltered,
                TotalCourses = totalCourses,
                ActiveCourses = activeCourses,
                TotalEnrolled = totalEnrolled,
                TotalHours = totalHours
            };

            return View(viewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseCode,CourseName,Description,DurationHours,Status,Type,Certification,Prerequisites,LearningObjectives,NQFLevel,Credits,UnitStandardNumber")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseCode,CourseName,Description,DurationHours,Status,Type,Certification,Prerequisites,LearningObjectives,NQFLevel,Credits,UnitStandardNumber")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
