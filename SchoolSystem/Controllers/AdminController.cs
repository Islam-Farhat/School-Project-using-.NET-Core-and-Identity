using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;
using SchoolSystem.Repository;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository iadminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAdminRepository iadminRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.iadminRepository = iadminRepository;
            this._webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AddTeacher()
        {
            ViewBag.flag = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(TeacherViewModel teacherVM, IFormFile Photo)
        {

            if (ModelState.IsValid)
            {
                //Upload file
                string filename = string.Empty;
                if (Photo != null)
                {
                    string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    filename = Photo.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    Photo.CopyTo(new FileStream(fullpath, FileMode.Create));
                }

                bool result = await iadminRepository.AddTeacher(teacherVM);
                if (result)
                {
                    ViewBag.flag = true;
                }
                return View();
            }

            return View(teacherVM);
        }

        public async Task<IActionResult> AddStudent()
        {
            StudentViewModel studentVM = new StudentViewModel();
            studentVM.Classes = await iadminRepository.GetClasses();
            studentVM.Levels = await iadminRepository.GetLevels();
            ViewBag.flag = false;
            return View(studentVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentViewModel studentVM, IFormFile Photo)
        {
            //ignore list levels and classes to make modelstate valid
            ModelState.Remove("Levels");
            ModelState.Remove("Classes");

            if (ModelState.IsValid)
            {
                //Upload file
                string filename = string.Empty;
                if (Photo != null)
                {
                    string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    filename = Photo.FileName;
                    string fullpath = Path.Combine(uploads, filename);
                    Photo.CopyTo(new FileStream(fullpath, FileMode.Create));
                }

                bool result = await iadminRepository.AddStudent(studentVM);
                if (result)
                {
                    ViewBag.flag = true;
                }
                return RedirectToAction("AddStudent");
            }

            return RedirectToAction("AddStudent");
        }
    }
}
