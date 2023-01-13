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
    public class UsersController : Controller
    {
        private readonly DBContext _context;
        public User? user;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UsersLogin()
        {
            HttpContext.Session.Clear();
            return View();
        }


        [HttpPost]
        public IActionResult UsersLogin(User model)
        {
            if (ModelState.IsValid)
            {
                user = _context.Users.FirstOrDefault(user => user.Username.Equals(model.Username)
                 && user.Password.Equals(model.Password));

                if (user != null)
                {
                    String role = user.Role;
                    HttpContext.Session.SetString(role, user.Username);
                    return RedirectToAction("Index", role + "s");
                }

                ModelState.AddModelError(string.Empty, "Δέν βρέθηκε χρήστης με αυτά τα στοιχεία");
            }

            return View();
        }

    }
}
