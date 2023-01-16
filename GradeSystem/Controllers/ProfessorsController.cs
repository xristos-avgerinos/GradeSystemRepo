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

        // GET: Professor
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Professor") != null)
            {
                String username = HttpContext.Session.GetString("Professor");
                var professor = _context.Professors.FirstOrDefault(s => s.Username == username);

                return View(professor);
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult SelectByLesson()
        {
            if (HttpContext.Session.GetString("Professor") != null)
            {
                String username = HttpContext.Session.GetString("Professor");

                ProfessorLessons(username);      
                
                return View();
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> SelectByLesson(int SelectedCourseId)
        {
            if (HttpContext.Session.GetString("Professor") != null)
            {
                String username = HttpContext.Session.GetString("Professor");

                ProfessorLessons(username);

                var gradesByLesson = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).ThenInclude(s => s.Professor).Where(s => s.Course.Professor.Username.Equals(username) && s.GradeCourseStudent != null && s.IdCourse == SelectedCourseId);
                ViewBag.Selected = SelectedCourseId;

                return View(await gradesByLesson.ToListAsync());
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> InsertGrades()
        {
            if (HttpContext.Session.GetString("Professor") != null)
            {
                String username = HttpContext.Session.GetString("Professor");

                ProfessorLessons(username);

                return View();
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> InsertGrades(int SelectedCourseId, int? RegNum, int? grade)
        {
            if (HttpContext.Session.GetString("Professor") != null)
            {
                String username = HttpContext.Session.GetString("Professor");

                ProfessorLessons(username);

                if (RegNum != null && grade != null)
                {

                    CourseHasStudent crs = new CourseHasStudent();

                    crs = _context.CourseHasStudents.FirstOrDefault(u => u.RegistrationNumber == RegNum && u.IdCourse == SelectedCourseId);
                    crs.GradeCourseStudent = grade;
                    _context.Update(crs);
                    _context.SaveChanges();
                }
                var gradesByLesson = _context.CourseHasStudents.Include(s => s.Student).Include(s => s.Course).ThenInclude(s => s.Professor).Where(s => s.Course.Professor.Username.Equals(username) && s.GradeCourseStudent == null && s.IdCourse == SelectedCourseId);
                ViewBag.Selected = SelectedCourseId;

                return View(await gradesByLesson.ToListAsync());
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }

        }

        public void ProfessorLessons(String username) 
        {
            var lessons = _context.Courses.Include(s => s.Professor).Where(s => s.Professor.Username.Equals(username)).Select(s => new { s.IdCourse, s.CourseTitle });
            ViewBag.lessonsList = lessons.ToList();
        }

        public IActionResult LogOut()
        {
            return RedirectToAction("UsersLogin", "Users");
        }
    }
}
