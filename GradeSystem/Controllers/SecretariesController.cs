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
    public class SecretariesController : Controller
    {
        private readonly DBContext _context;

        public SecretariesController(DBContext context)
        {
            _context = context;
        }

        // GET: Secretaries
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("Secretarie") != null)
            {
                String username = HttpContext.Session.GetString("Secretarie");
                var secretary = _context.Secretaries.FirstOrDefault(s => s.Username == username);
                String department = secretary.Department;
                HttpContext.Session.SetString("department", department);

                return View(secretary);
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> SelectCourses()
        {
            if (HttpContext.Session.GetString("Secretarie") != null)
            {
                String username = HttpContext.Session.GetString("Secretarie");
                string department = HttpContext.Session.GetString("department");


                var courses = _context.Courses.Include(s => s.Professor).Where(s => s.Professor.Department.Equals(department) && s.Afm != null);
                

                return View(await courses.ToListAsync());
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        
        public IActionResult InsertCourses()
        {

            if (HttpContext.Session.GetString("Secretarie") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertCourses(Course course)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(course);
        }




        /*
                // GET: Secretaries/Details/5
                public async Task<IActionResult> Details(int? id)
                {
                    if (id == null || _context.Secretaries == null)
                    {
                        return NotFound();
                    }

                    var secretary = await _context.Secretaries
                        .Include(s => s.User)
                        .FirstOrDefaultAsync(m => m.Phonenumber == id);
                    if (secretary == null)
                    {
                        return NotFound();
                    }

                    return View(secretary);
                }

                // GET: Secretaries/Create
                public IActionResult Create()
                {
                    ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
                    return View();
                }

                // POST: Secretaries/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("Phonenumber,Name,Surname,Department,Username")] Secretary secretary)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(secretary);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", secretary.Username);
                    return View(secretary);
                }

                // GET: Secretaries/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null || _context.Secretaries == null)
                    {
                        return NotFound();
                    }

                    var secretary = await _context.Secretaries.FindAsync(id);
                    if (secretary == null)
                    {
                        return NotFound();
                    }
                    ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", secretary.Username);
                    return View(secretary);
                }

                // POST: Secretaries/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("Phonenumber,Name,Surname,Department,Username")] Secretary secretary)
                {
                    if (id != secretary.Phonenumber)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(secretary);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!SecretaryExists(secretary.Phonenumber))
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
                    ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", secretary.Username);
                    return View(secretary);
                }

                // GET: Secretaries/Delete/5
                public async Task<IActionResult> Delete(int? id)
                {
                    if (id == null || _context.Secretaries == null)
                    {
                        return NotFound();
                    }

                    var secretary = await _context.Secretaries
                        .Include(s => s.User)
                        .FirstOrDefaultAsync(m => m.Phonenumber == id);
                    if (secretary == null)
                    {
                        return NotFound();
                    }

                    return View(secretary);
                }

                // POST: Secretaries/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    if (_context.Secretaries == null)
                    {
                        return Problem("Entity set 'DBContext.Secretaries'  is null.");
                    }
                    var secretary = await _context.Secretaries.FindAsync(id);
                    if (secretary != null)
                    {
                        _context.Secretaries.Remove(secretary);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                private bool SecretaryExists(int id)
                {
                  return _context.Secretaries.Any(e => e.Phonenumber == id);
                }*/
    }
}
