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
        private readonly IClassRepository iclassRepository;
        private readonly IlevelRepository ilevelRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAdminRepository iadminRepository, IClassRepository iclassRepository, IlevelRepository ilevelRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.iadminRepository = iadminRepository;
            this.iclassRepository = iclassRepository;
            this.ilevelRepository = ilevelRepository;
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
            studentVM.Classes = await iclassRepository.GetClasses();
            studentVM.Levels = await ilevelRepository.GetLevels();
            ViewBag.flag = false;
            return View(studentVM);
        }


        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentViewModel studentVM, IFormFile Photo)
        {
            //ignore list levels and classes to make modelstate valid
            ModelState.Remove("Levels");
            ModelState.Remove("Classes");
            ModelState.Remove("Id");
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

                bool result = await iadminRepository.AddStudent(studentVM);
                if (result)
                {
                    ViewBag.flag = true;
                }
                return RedirectToAction("AddStudent");
            }

            return RedirectToAction("AddStudent");
        }
        
        //test: 7dbbee52-2d44-4968-846e-74eef55c88fe
        public async Task<IActionResult> EditStudent(StudentViewModel studentVM)
        {
            var student = await iadminRepository.GetStudentByID(studentVM);
            student.Levels = await ilevelRepository.GetLevels();
            student.Classes = await iclassRepository.GetClasses();
            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(StudentViewModel studentVM, IFormFile Photo)
        {
            ModelState.Remove("Photo");
            ModelState.Remove("Gender");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Levels");
            ModelState.Remove("Classes");
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

                bool result = iadminRepository.UpdateStudent(studentVM);
                if (result)
                    return RedirectToAction("ShowStudentReports", "AttendanceController");
                    
            }
            return RedirectToAction("EditStudent");
        }

        //Level Actions
        public IActionResult AddLevel()
        {
            ViewBag.flag = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLevel(LevelViewModel levelVM)
        {
            ModelState.Remove("Id");
            ViewBag.flag = false;
            if (ModelState.IsValid)
            {
                bool result = await ilevelRepository.AddLevel(levelVM);
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
            var result = await ilevelRepository.GetLevels();
            return Json(result);
        }

        public IActionResult GetLevelByID(int? id)
        {
            var result = ilevelRepository.GetLevelByID(id);
            return Json(result);
        }

        public IActionResult UpdateLevel(LevelViewModel levelVM)
        {
            if (ModelState.IsValid)
            {
                var result = ilevelRepository.UpdateLevel(levelVM);
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
                var result = ilevelRepository.DeleteLevel(id);
                if (result)
                    return Json("success");
                else
                    return Json("error");
            }
            return View();
        }


        //Class Actions
        public async Task<IActionResult> AddClass()
        {
            ClassViewModel classVM = new ClassViewModel();
            classVM.Levels = await ilevelRepository.GetLevels();
            ViewBag.flag = false;
            return View(classVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddClass(ClassViewModel classVM)
        {
            ModelState.Remove("Id");
            ModelState.Remove("Levels");
            ViewBag.flag = false;
            if (ModelState.IsValid)
            {
                bool result = await iclassRepository.AddClass(classVM);
                if (result == true)
                {
                    ViewBag.flag = true;
                    return RedirectToAction("AddClass");
                }
            }
            return RedirectToAction("AddClass");
        }
        public async Task<IActionResult> GetAllClasses()
        {
            var result = await iclassRepository.GetClasses();
            return Json(result);
        }
        public IActionResult GetClassByID(int? id)
        {
            var result = iclassRepository.GetClassByID(id);
            return Json(result);
        }
        public IActionResult UpdateClass(ClassViewModel classVM)
        {
            ModelState.Remove("Levels");
            if (ModelState.IsValid)
            {
                var result = iclassRepository.UpdateClass(classVM);
                if (result)
                    return Json("success");
                else
                    return Json("error");
            }
            return View(classVM);
        }
        public IActionResult DeleteClass(int? id)
        {
            ModelState.Remove("Levels");
            if (ModelState.IsValid)
            {
                var result = iclassRepository.DeleteClass(id);
                if (result)
                    return Json("success");
                else
                    return Json("error");
            }
            return View();
        }


        //Feedback
        public IActionResult Feedback()
        {
            return View(iadminRepository.GetFeedbacks());
        }
        public IActionResult Replay(int? id)
        {
            if (id == null)
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Replay(FeedbackVM feedback)
        {
            ModelState.Remove("FeedbackText");
            ModelState.Remove("StudentName");
            if (ModelState.IsValid)
            {
                bool result = iadminRepository.AddFeedback(feedback);
                if (result == true)
                {
                    return RedirectToAction("Feedback");
                }
            }
            return View();
        }
    }
}
