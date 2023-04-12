using App.Repos;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;
using SchoolSystem.Repository;
using SchoolSystem.ViewModels;
using System.Security.Claims;

namespace SchoolSystem.Controllers
{
    public class TeacherController : Controller
    {
       

        private readonly IRepository<ApplicationUser> _teacherRepository;
        private readonly IUserRepo _userRepo;

        public TeacherController(IRepository<ApplicationUser> teacherRepository, IUserRepo userRepo)
        {
            _teacherRepository = teacherRepository;
            _userRepo = userRepo;
        }

        //[HttpGet]
        public async Task<IActionResult> UpdateTeacher()
        {
            string teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser teacher = await _userRepo.GetTeacherByIdAsync(teacherId);

            if(teacher == null) { return BadRequest(); }

            TeacherViewModel teacherView = new TeacherViewModel()
            {   Name = teacher.Name,
                Address = teacher.Address,
                Phone = teacher.PhoneNumber,
                BirthDate = teacher.BirthDate,
                UserName = teacher.UserName,
                Email = teacher.Email,
                Password = teacher.PasswordHash,
            };
           
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
