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
    public class EmployersController : Controller
    {
        private readonly AcademyDbContext _context;

        public EmployersController(AcademyDbContext context)
        {
            _context = context;
        }

        // GET: Employers
        // GET: Employers
        public async Task<IActionResult> Index(string? searchTerm, string? sector, string? province, string? status, int page = 1)
        {
            const int pageSize = 3;

            var allEmployers = _context.Employers.AsQueryable();
            var totalEmployers = await allEmployers.CountAsync();
            var activeCount = await allEmployers.CountAsync(e => e.Status == "Active");
            var inactiveCount = await allEmployers.CountAsync(e => e.Status == "InActive");
            var pendingCount = await allEmployers.CountAsync(e => e.Status == "Pending");

            var query = _context.Employers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.CompanyName.Contains(searchTerm) ||
                    (e.ContactPerson != null && e.ContactPerson.Contains(searchTerm)) ||
                    (e.City != null && e.City.Contains(searchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(sector))
            {
                query = query.Where(e => e.Sector == sector);
            }

            if (!string.IsNullOrWhiteSpace(province))
            {
                query = query.Where(e => e.Province == province);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status == status);
            }

            var totalFiltered = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

            var employers = await query
                .OrderBy(e => e.CompanyName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var sectors = await _context.Employers
                .Select(e => e.Sector)
                .Distinct()
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .OrderBy(s => s)
                .ToListAsync();

            var provinces = new List<string>
    {
        "Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal",
        "Limpopo", "Mpumalanga", "North West", "Northern Cape", "Western Cape"
    };

            var viewModel = new EmployerIndexViewModel
            {
                Employers = employers,
                SearchTerm = searchTerm,
                Sector = sector,
                Province = province,
                Status = status,
                Sectors = sectors,
                Provinces = provinces,
                Statuses = new List<string> { "Active", "InActive", "Pending" },
                PageIndex = page,
                TotalPages = totalPages,
                TotalFilteredEmployers = totalFiltered,
                TotalEmployers = totalEmployers,
                ActiveCount = activeCount,
                InactiveCount = inactiveCount,
                PendingCount = pendingCount
            };

            return View(viewModel);
        }

        // GET: Employers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        private void PopulateDropdowns()
        {
            ViewBag.StatusOptions = new List<string> { "Active", "InActive", "Pending" };
            ViewBag.ProvinceOptions = new List<string>
            {
             "Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal",
             "Limpopo", "Mpumalanga", "North West", "Northern Cape", "Western Cape"
            };
        }

        // GET: Employers/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Employers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,ContactPerson,ContactEmail,ContactPhone,Sector,Size,City,Province,Status")] Employer employer)
        {
            // DEBUG: expose exactly which fields are failing
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });
                foreach (var error in errors)
                {
                    Console.WriteLine($"Field: {error.Field} → {string.Join(", ", error.Errors)}");
                }
                PopulateDropdowns();
                return View(employer);
            }

            try
            {
                _context.Add(employer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unable to save: {ex.Message}");
            }

            PopulateDropdowns();
            return View(employer);
        }

        // GET: Employers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }

            PopulateDropdowns();
            
            return View(employer);
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,ContactPerson,ContactEmail,ContactPhone,Sector,Size,City,Province,Status")] Employer employer)
        {
            if (id != employer.Id)
            {
                return NotFound();
            }

            PopulateDropdowns();

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });
                foreach (var error in errors)
                {
                    Console.WriteLine($"Field: {error.Field} → {string.Join(", ", error.Errors)}");
                }
                return View(employer);
            }

            try
            {
                _context.Update(employer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployerExists(employer.Id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unable to save changes: {ex.Message}");
            }

            return View(employer);
        }

        // GET: Employers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer != null)
            {
                _context.Employers.Remove(employer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployerExists(int id)
        {
            return _context.Employers.Any(e => e.Id == id);
        }
    }
}
