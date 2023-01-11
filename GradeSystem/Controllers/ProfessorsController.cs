using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradeSystem.Models;

namespace GradeSystem.Controllers
{
    public class ProfessorsController : Controller
    {
        private readonly DBContext _context;

        public ProfessorsController(DBContext context)
        {
            _context = context;
        }

        // GET: Professors
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.Professors.Include(p => p.UsernameNavigation);
            return View(await dBContext.ToListAsync());
        }

        // GET: Professors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Professors == null)
            {
                return NotFound();
            }

            var professor = await _context.Professors
                .Include(p => p.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Afm == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // GET: Professors/Create
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        // POST: Professors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Afm,Name,Surname,Department,Username")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", professor.Username);
            return View(professor);
        }

        // GET: Professors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Professors == null)
            {
                return NotFound();
            }

            var professor = await _context.Professors.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", professor.Username);
            return View(professor);
        }

        // POST: Professors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Afm,Name,Surname,Department,Username")] Professor professor)
        {
            if (id != professor.Afm)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.Afm))
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
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", professor.Username);
            return View(professor);
        }

        // GET: Professors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Professors == null)
            {
                return NotFound();
            }

            var professor = await _context.Professors
                .Include(p => p.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Afm == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // POST: Professors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Professors == null)
            {
                return Problem("Entity set 'DBContext.Professors'  is null.");
            }
            var professor = await _context.Professors.FindAsync(id);
            if (professor != null)
            {
                _context.Professors.Remove(professor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorExists(int id)
        {
          return _context.Professors.Any(e => e.Afm == id);
        }
    }
}
