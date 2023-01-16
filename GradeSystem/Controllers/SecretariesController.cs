using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GradeSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System.Diagnostics;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> InsertCourses(Course course)
        {
            if (HttpContext.Session.GetString("Secretarie") != null)
            {

                if (ModelState.IsValid)
                {

                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(course);
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult InsertProfessors()
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> InsertProfessors(Professor professor)
        {
            if (HttpContext.Session.GetString("Secretarie") != null)
            {
                professor.Username = "p" + professor.Afm;
                User user = _context.Users.FirstOrDefault(u => u.Username.Equals(professor.Username));

                if (user == null)
                {

                    user = new User();
                    user.Username = professor.Username;
                    user.Password = professor.Username;
                    user.Role = "Professor";
                    professor.User = user;

                    _context.Add(professor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Το ΑΦΜ υπάρχει ήδη");
                }

                return View(professor);

            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult InsertStudents()
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> InsertStudents(Student student)
        {
            if (HttpContext.Session.GetString("Secretarie") != null)
            {

                User user = _context.Users.FirstOrDefault(u => u.Username.Equals(student.Username));

                if (user == null)
                {

                    user = new User();
                    user.Username = student.Username;
                    user.Password = student.Username;
                    user.Role = "Student";
                    student.User = user;

                    _context.Add(student);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Το username υπάρχει ήδη");
                }

                return View(student);

            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult AssignCourseToProfessor()
        {
            if (HttpContext.Session.GetString("Secretarie") != null)
            {
                CoursesListWithoutProfessor();
                return View();
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
           
            
        }
        
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult AssignCourseToProfessor(int SelectedCourseId,int? SelectedAfm)
        {

            if (HttpContext.Session.GetString("Secretarie") != null)
            {

                Course course = _context.Courses.FirstOrDefault(s => s.IdCourse == SelectedCourseId);
                

                if (SelectedAfm != null)
                {

                    course.Afm = SelectedAfm;
                    _context.Update(course);
                    _context.SaveChanges();

                    CoursesListWithoutProfessor();
                    return View();


                }

                CoursesListWithoutProfessor();
                string department = HttpContext.Session.GetString("department");
                var professors = _context.Professors.Where(s=>s.Department.Equals(department)).Select(s => new { s.Name, s.Surname, s.Afm });
                ViewBag.professors = professors.ToList();

                ViewBag.Selected = SelectedCourseId;


                return View(course);
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }
           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult AssignCourseToStudent()
        {
            if (HttpContext.Session.GetString("Secretarie") != null)
            {
                AllStudents();

                return View();
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }


        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.Any, NoStore = true)]
        public IActionResult AssignCourseToStudent(int SelectedStudentId, int? SelectedCourseId)
        {

            if (HttpContext.Session.GetString("Secretarie") != null)
            {

                
                if (SelectedCourseId != null)
                {

                    CourseHasStudent cs = new CourseHasStudent();
                    cs.IdCourse = (int)SelectedCourseId;
                    cs.RegistrationNumber = SelectedStudentId;

                    _context.Add(cs);
                    _context.SaveChanges();


                }

                Student student = _context.Students.FirstOrDefault(s => s.RegistrationNumber == SelectedStudentId);
                AllStudents();

                string department = HttpContext.Session.GetString("department");
                var temp_lessons1 = _context.Courses.Include(c => c.Professor).Where(c => c.Afm != null && c.Professor.Department.Equals(department)).Select(c => new { c.IdCourse, c.CourseTitle });


                var temp_lessons2 = _context.CourseHasStudents.Include(s=>s.Course).Where(s => s.RegistrationNumber == SelectedStudentId).Select(c=> new { c.IdCourse, c.Course.CourseTitle });

                var lessons = temp_lessons1.Except(temp_lessons2);
                ViewBag.Courses = lessons.ToList();

                ViewBag.Selected = SelectedStudentId;


                return View(student);
            }
            else
            {
                return RedirectToAction("UsersLogin", "Users");
            }

        }

        void CoursesListWithoutProfessor()
        {
            var lessons = _context.Courses.Where(s => s.Afm == null).Select(s => new { s.IdCourse, s.CourseTitle });
            ViewBag.Courses = lessons.ToList();

        }


        void AllStudents()
        {
            string department = HttpContext.Session.GetString("department");
            var Students = _context.Students.Where(s => s.Department.Equals(department)).Select(s => new { s.RegistrationNumber, s.Name, s.Surname });
            ViewBag.Students = Students.ToList();
        }

    }
}
