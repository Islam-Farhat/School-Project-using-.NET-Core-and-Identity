using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;
using SchoolSystem.Repository;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAdminRepository iadminRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.adminRepository = iadminRepository;
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

                bool result = await adminRepository.AddTeacher(teacherVM);
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
            studentVM.Classes = await adminRepository.GetClasses();
            studentVM.Levels = await adminRepository.GetLevels();
            ViewBag.flag = false;
            return View(studentVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentViewModel studentVM, IFormFile Photo)
        {
            //ignore list levels and classes to make modelstate valid
            ModelState.Remove("Levels");
            ModelState.Remove("Classes");
            ViewBag.flag = false;

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

                bool result = await adminRepository.AddStudent(studentVM);
                if (result)
                {
                    ViewBag.flag = true;
                }
                return RedirectToAction("AddStudent");
            }

            return RedirectToAction("AddStudent");
        }

        public IActionResult AddLevel()
        {
            ViewBag.flag = false;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddLevel(LevelViewModel level)
        {
            ModelState.Remove("Id");
            ViewBag.flag = false;
            if (ModelState.IsValid)
            {
                bool result = await adminRepository.AddLevel(level);
                if (result == true)
                {
                    ViewBag.flag = true;
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> GetAllLevels()
        {
            var result = await adminRepository.GetLevels();
            return Json(result);
        }

        public IActionResult GetLevelByID(int? id)
        {
            var result = adminRepository.GetLevelByID(id);
            return Json(result);
        }

        public IActionResult UpdateLevel(LevelViewModel levelVM)
        {
            if (ModelState.IsValid)
            {
                var result = adminRepository.UpdateLevel(levelVM);
                if (result)
                    return Json("success");
                else
                    return Json("error");
            }
            return View(levelVM); 
        }
        public IActionResult DeleteLevel(int? id)
        {
            if (ModelState.IsValid)
            {
                var result = adminRepository.DeleteLevel(id);
                if (result)
                    return Json("success");
                else
                    return Json("error");
            }
            return View();
        }
    }
}
