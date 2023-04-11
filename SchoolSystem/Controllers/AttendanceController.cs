using App.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Migrations;
using SchoolSystem.Models;
using SchoolSystem.Repository;
using SchoolSystem.Services;
using SchoolSystem.ViewModels;
using System.Runtime.Intrinsics.X86;

namespace SchoolSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IUserRepo _userRepository;
        private readonly IHolidayService _holidayService;
        private readonly ILevelService _levelService;
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService, IUserRepo userRepository,ILevelService levelService, IHolidayService holidayService)
        {
            _attendanceService = attendanceService;
            _userRepository = userRepository;
            _levelService = levelService;
            _holidayService = holidayService;
        }


        // GET: AttendanceController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult ShowHolidaysRequests()
        {
            List<Holiday> holidays = _holidayService.GetAllHolidays();
            return View(holidays );

        }
        
        public ActionResult HolidayRequestResponse(int id,int status)
        {
            Holiday holiday = _holidayService.GetHolidayById(id);
            holiday.Status = (StatusType)status;
            _holidayService.updateHoliday(holiday);
            _holidayService.Save();

            return RedirectToAction("ShowHolidaysRequests");
        }






        [HttpGet]
        public ActionResult StudentsReports ()
        {
            SelectStudentsViewModel vm = new SelectStudentsViewModel();
            vm.levels = _levelService.GetAllLevels();
            return View(vm);
        }

        public async Task<IActionResult> ShowStudentReports(int classId, int levelId)
        {
            var students = await _userRepository.GetStudentsByClassAndLevelAsync(classId, levelId);
            return PartialView(students);
        }


        [HttpGet]
        public ActionResult AttendancePage ()
        {
            SelectStudentsViewModel vm = new SelectStudentsViewModel();
            vm.levels = _levelService.GetAllLevels();
            return View(vm);
        }

        public async Task<IActionResult> TakeAttendance(int classId, int levelId)
        {
            var students = await _userRepository.GetStudentsByClassAndLevelAsync(classId, levelId);
            return PartialView("TakeAttendance", students);
        }

        [HttpPost]
        public ActionResult TakeAttendance(List<ApplicationUser> students)
        {

            if (ModelState.IsValid)
            { 
                var attendanceStatuses = Request.Form["AttendanceStatus"];
                var studentIds = Request.Form["student.Id"];

                for (int i = 0; i < attendanceStatuses.Count; i++)
                {
                    Attendance attendance = new Attendance();
                    AttendanceStatus status;
                    if (Enum.TryParse(attendanceStatuses[i], out status))
                    {
                        attendance.AttendanceStatus = status;
                    }
                    else
                    {
                        attendance.AttendanceStatus = AttendanceStatus.Present;
                       
                    }

                    attendance.userID_fk = studentIds[i];
                    _attendanceService.AddAttendance(attendance);
                }
                _attendanceService.Save();

                return RedirectToAction("Index", "Home");
            }

            return PartialView("TakeAttendance", students);

        }

        public ActionResult AttendanceListLevels()
        {
            SelectStudentsViewModel vm = new SelectStudentsViewModel();
            vm.levels = _levelService.GetAllLevels();
            return View(vm);
        }



        public async Task<IActionResult> AttendanceList(int levelId, int classId, DateTime date)
        {

            var students = await _userRepository.GetStudentsByClassAndLevelAsync(classId, levelId);
            var attendanceRecords = await _attendanceService.GetAttendacesByDate(levelId, classId, date);

            var studentAttendanceList = new List<StudentAttendanceViewModel>();

            if (attendanceRecords.Count >0 )
            {
                foreach (var student in students)
                {
                    var attendanceRecord = attendanceRecords.FirstOrDefault(a => a.userID_fk == student.Id);

                    var attendanceState = AttendanceStatus.Absent;
                    if (attendanceRecord != null)
                    {
                        attendanceState = (AttendanceStatus)attendanceRecord.AttendanceStatus;
                    }

                    studentAttendanceList.Add(new StudentAttendanceViewModel
                    {
                        Student = student,
                        AttendanceStatus = attendanceState
                    });
                }
            }
            else{

                studentAttendanceList.Clear(); 
            }


            return PartialView(studentAttendanceList);
        }


        [HttpGet]
        public IActionResult GetClassessByLevelID(int id)
        {
            List<Classes> classes = _levelService.GetLevelById(id).Classes;

            return Json(classes);
        }

    }
}
