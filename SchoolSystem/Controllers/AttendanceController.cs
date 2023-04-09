using App.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;
using SchoolSystem.Repository;
using SchoolSystem.ViewModels;
using System.Runtime.Intrinsics.X86;

namespace SchoolSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IUserRepo _userRepository;
        private readonly ILevelService _levelService;

        public AttendanceController(IRepository<Attendance> attendanceRepository, IUserRepo userRepository,ILevelService levelService)
        {
            _attendanceRepository = attendanceRepository;
            _userRepository = userRepository;
            _levelService = levelService;
        }


        public async Task<IActionResult> TakeAttendance(int classId, int levelId)
        {
            var students =  await _userRepository.GetStudentsByClassAndLevelAsync(classId, levelId);
            List<Attendance> attendanceList = new List<Attendance>();
            foreach (var student in students)
            {
                var attendance = new Attendance();
                attendance.userID_fk = student.Id;
                attendance.AttendanceStatus = student.attendanceStatus;
                attendanceList.Add(attendance);
            }

            return View(attendanceList);
        }


        [HttpPost]
        public ActionResult TakeAttendance(List<Attendance> attendanceList)
        {
            foreach (var attendance in attendanceList)
            {
                _attendanceRepository.Insert(attendance);
            }

            _attendanceRepository.Save();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetClassessByLevelID(int id)
        {
            List<Classes> classes=_levelService.GetLevelById(id).Classes;

            return Json(classes);
        }


        // GET: AttendanceController
        public ActionResult Index()
        {
            return View();
        }


        // GET: AttendanceController/Create
        [HttpGet]
        public ActionResult Create()
        {
            AttendanceViewModel vm = new AttendanceViewModel();
            vm.levels = _levelService.GetAllLevels();

            return View(vm);
        }

        // POST: AttendanceController/Create

        // [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(List<Attendance> attendances)
        {
             if (ModelState.IsValid)
            {
                foreach (Attendance attendance in attendances) {
                    _attendanceRepository.Insert(attendance);
                }

                return RedirectToAction("Index", "Home");

            }
            AttendanceViewModel vm = new AttendanceViewModel();
            vm.levels = _levelService.GetAllLevels();


            return View(vm);
        }

        // GET: AttendanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: AttendanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
