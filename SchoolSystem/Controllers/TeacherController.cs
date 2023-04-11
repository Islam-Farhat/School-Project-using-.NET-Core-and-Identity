using App.Repos;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;
using SchoolSystem.Repository;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Controllers
{
    public class TeacherController : Controller
    {
       

        private readonly ITeacherRepo _teacherRepository;

        public TeacherController(ITeacherRepo iTeacherRepo )
        {
            _teacherRepository = iTeacherRepo;
        }

        //[HttpGet]
        public IActionResult UpdateTeacher(int id)
        {
            if(id == 0)
            { return BadRequest(); }
            
            var teacher = _teacherRepository.GetById(id);
            if(teacher == null) { return BadRequest(); }

            TeacherViewModel teacherView = new TeacherViewModel();
            teacherView.Name = teacher.Name;
            teacherView.Address = teacher.Address;
            teacherView.Phone = teacher.PhoneNumber;
            teacherView.BirthDate = teacher.BirthDate;
            teacherView.UserName = teacher.UserName;
            teacherView.Email = teacher.Email;
           // teacherView.Photo = teacher.photoUrl;
            teacherView.BirthDate= teacher.BirthDate;
            teacherView.Password = teacher.PasswordHash;
            teacherView.ConfirmPassword = teacher.PasswordHash;

            return View("UpdateTeacher", teacherView);
        }

        [HttpPost]
        public IActionResult UpdateTeacher( ApplicationUser teacher )
        {
            if (ModelState.IsValid)
            {
                _teacherRepository.Update(teacher);
                _teacherRepository.Save();
                return RedirectToAction("Index");


            }
            
           
            return View(teacher);
        }
    }
}
