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

        public AdminController(IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.adminRepository = adminRepository;
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

               bool result= await adminRepository.AddTeacher(teacherVM);
                if (result) {
                    ViewBag.flag = true;
                }
                return View();
            }
            
            return View(teacherVM);
        }
    }
}
