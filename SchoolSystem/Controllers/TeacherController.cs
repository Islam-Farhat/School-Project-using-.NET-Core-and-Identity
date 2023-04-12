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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepo _userRepo;

        public TeacherController(IRepository<ApplicationUser> teacherRepository, IUserRepo userRepo,IWebHostEnvironment webHostEnvironment)
        {
            _teacherRepository = teacherRepository;
            _userRepo = userRepo;
           _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> UpdateTeacher()
        {
            string teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser teacher = await _userRepo.GetTeacherByIdAsync(teacherId);

            TeacherViewModel teacherView = new TeacherViewModel()
            { Name = teacher.Name,
                Address = teacher.Address,
                Phone = teacher.PhoneNumber,
                BirthDate = teacher.BirthDate,
                UserName = teacher.UserName,
                Email = teacher.Email,
                Password = teacher.PasswordHash,
               // Photo.FileName = teacher.photoUrl
            };
           
            return Json( teacherView);
        }

        [HttpPost]
        public IActionResult UpdateTeacher(ApplicationUser teacher, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                string filename = string.Empty;
                if (Photo != null)
                {
                    string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    filename = Photo.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    Photo.CopyTo(new FileStream(fullpath, FileMode.Create));
                }

                _teacherRepository.Update(teacher);
                _teacherRepository.Save();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }
    }
}
