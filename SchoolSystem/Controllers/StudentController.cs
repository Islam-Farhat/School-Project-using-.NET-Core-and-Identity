using App.Repos;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IAttendanceService _attendanceService;
        public StudentController (IAttendanceService attendanceService,IRepository<Holiday> holidayRepository, IRepository<Feedback> feedbackRepository)
        {
            _holidayRepository = holidayRepository;
            _feedbackRepository = feedbackRepository;
            _attendanceService = attendanceService;
        }

        public IActionResult AttendanceReportMonth()
        { 
            return View();
        }
        public IActionResult AttendanceReport(int id)
        {
            int month = int.Parse(Request.Form["month"]);

            return View();
        }
        public IActionResult SendFeedback()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendFeedback(Feedback feedback)
        {
            if(ModelState.IsValid)
                return View("AttendanceReportMonth");
            return View();
        }
        public IActionResult ApplyHoliday()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ApplyHoliday(Holiday holiday)
        {
            if(ModelState.IsValid)
                return View("AttendanceReportMonth");
            return View(holiday);
        }
        public IActionResult ViewFeedbacks()
        {
            var feedbacks=_feedbackRepository.GetAll();
            return View(feedbacks);
        }
        public IActionResult ViewHolidays()
        {
            var holidays=_holidayRepository.GetAll();
            return View(holidays);
        }
        public IActionResult CancelHoliday()
        {
            return View();
        }
        public IActionResult EditHoliday()
        {
            return View();
        }
        public IActionResult CancelFeedback()
        {
            return View();
        }
        public IActionResult EditFeedback()
        {
            return View();
        }
        public IActionResult UpdateProfile()
        {
            return View();
        }
        public IActionResult UpdatePassword() 
        {
            return View();
        }



    }
}
