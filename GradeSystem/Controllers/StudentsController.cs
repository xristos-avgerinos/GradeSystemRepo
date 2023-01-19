using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradeSystem.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace GradeSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DBContext _context;
        

        public StudentsController(DBContext context)
        {
            _context = context;
        }

        // GET: Students
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Student") != null)
            {
                String username = HttpContext.Session.GetString("Student");
                var student = _context.Students.FirstOrDefault(s => s.Username == username);

                return View(student);
            }
            else
            {
                return RedirectToAction("UsersLogin","Users");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> SelectAll()
        {
            if (HttpContext.Session.GetString("Student") != null)
            {
                String username = HttpContext.Session.GetString("Student");

                var allGrades = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).
                    Where(s=>s.Student.Username.Equals(username) && s.GradeCourseStudent!=null);

                return View(await allGrades.ToListAsync());
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> SelectBySemester()
        {
            if (HttpContext.Session.GetString("Student") != null)
            {
                String username = HttpContext.Session.GetString("Student");

                string max_semester = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).
                    Where(s => s.Student.Username.Equals(username) && s.GradeCourseStudent != null).Max(s=>s.Course.CourseSemester);

                if (max_semester.IsNullOrEmpty())
                {
                    ViewBag.max_sem = 0;
                }
                else
                {
                    ViewBag.max_sem = Int32.Parse(max_semester);
                    HttpContext.Session.SetInt32("max_sem", Int32.Parse(max_semester));
                }
                

                var gradesBySemester = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).
                    Where(s=>s.Course.CourseSemester.Equals("1") && s.Student.Username.Equals(username) && s.GradeCourseStudent != null);

                ViewBag.Selected = 1;

                return View(await gradesBySemester.ToListAsync());
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> SelectBySemester(int Selected)
        {
            if (HttpContext.Session.GetString("Student") != null)
            {
                String username = HttpContext.Session.GetString("Student");
                ViewBag.Selected = Selected;
                ViewBag.max_sem = HttpContext.Session.GetInt32("max_sem");

                var gradesBySemester = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).
                    Where(s => s.Course.CourseSemester.Equals(Selected.ToString()) && s.Student.Username.Equals(username) && s.GradeCourseStudent != null);

                return View(await gradesBySemester.ToListAsync());
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult GradesAverage()
        {
            if (HttpContext.Session.GetString("Student") != null)
            {
                String username = HttpContext.Session.GetString("Student");

                var gradesAvg = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).
                    Where(s => s.Student.Username.Equals(username) && s.GradeCourseStudent >= 5).Average(s=>s.GradeCourseStudent);

                int coursesCount = _context.CourseHasStudents.Include(s => s.Student).
                    Where(s => s.Student.Username.Equals(username) && s.GradeCourseStudent>=5).Count();

                ViewBag.coursesCount = coursesCount;
                if (gradesAvg == null)
                {
                    ViewBag.gradesAvg = 0;
                }
                else
                {
                    ViewBag.gradesAvg = System.Math.Round((double)gradesAvg, 2);
                }

                return View();
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("UsersLogin", "Users");
        }

    }
}
