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
        private readonly string testUserId = "salma";//"4479b419-66ea-417d-9f04-2a83ea1cd27a";

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
        public IActionResult AttendanceReport()
        {
            int month = int.Parse(Request.Form["month"]);
            List<Attendance> attendances = _attendanceService.GetAttendacesByStdId(testUserId, month);
            return View(attendances);
        }
        public IActionResult SendFeedback()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendFeedback(FeedbackViewModel feedbackVM)
        {
            if (ModelState.IsValid)
            {
                Feedback feedback=new Feedback();
                feedback.userID_fk= testUserId;
                feedback.FeedbackText = feedbackVM.FeedbackTxt;
                _feedbackRepository.Insert(feedback);
                _feedbackRepository.Save();
                return RedirectToAction("ViewFeedbacks");
            }
            return View();
        }
        public IActionResult ViewFeedbacks()
        {
            var feedbacks = _feedbackRepository.GetAll().Where(f=>f.userID_fk==testUserId).ToList();
            return View(feedbacks);
        }
        public IActionResult DeleteFeedback(int id)
        {
            _feedbackRepository.Delete(id);
            _feedbackRepository.Save();
            return RedirectToAction("ViewFeedbacks");
        }
        public IActionResult EditFeedback(int id)
        {
            Feedback feedback = _feedbackRepository.GetById(id);
            FeedbackViewModel feedbackVM= new FeedbackViewModel();
            feedbackVM.FeedbackTxt = feedback.FeedbackText;
            return View(feedbackVM);
        }
        [HttpPost]
        public IActionResult EditFeedback(int id,FeedbackViewModel feedbackVM)
        {
            if (ModelState.IsValid)
            {
                Feedback feedback = _feedbackRepository.GetById(id);
                feedback.FeedbackText = feedbackVM.FeedbackTxt;
                _feedbackRepository.Update(feedback);
                _feedbackRepository.Save();
                return RedirectToAction("ViewFeedbacks");
            }
            return View();
        }
        public IActionResult ApplyHoliday()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ApplyHoliday(HolidayViewModel holidayVM)
        {
            if (ModelState.IsValid)
            {
                Holiday holiday = new Holiday();
                holiday.userID_fk = testUserId;
                holiday.StartDate=holidayVM.StartDate;
                holiday.DaysNum = holidayVM.DaysNum;
                holiday.Reason = holidayVM.Reason;
                holiday.Status = StatusType.Pending;
                _holidayRepository.Insert(holiday);
                _holidayRepository.Save();
                return RedirectToAction("ViewHolidays");
            }    
            return View(holidayVM);
        }

        public IActionResult ViewHolidays()
        {
            var holidays = _holidayRepository.GetAll().Where(h=>h.userID_fk == testUserId).ToList();
            return View(holidays);
        }
        public IActionResult DeleteHoliday(int id)
        {
            Holiday holiday=_holidayRepository.GetById(id);
            if (holiday.Status == StatusType.Pending)
            {
                _holidayRepository.Delete(id);
                _holidayRepository.Save();
            }
            return RedirectToAction("ViewHolidays");
        }
        public IActionResult EditHoliday(int id)
        {
            Holiday holiday= _holidayRepository.GetById(id);
            HolidayViewModel holidayVM= new HolidayViewModel();
            holidayVM.StartDate = holiday.StartDate;
            holidayVM.DaysNum = holiday.DaysNum;
            holidayVM.Reason = holiday.Reason;
            return View(holidayVM);
        }
        [HttpPost]
        public IActionResult EditHoliday(int id,HolidayViewModel holidayVM)
        {
            if(ModelState.IsValid)
            {
                Holiday holiday = _holidayRepository.GetById(id);
                holiday.StartDate = holidayVM.StartDate;
                holiday.DaysNum = holidayVM.DaysNum;
                holiday.Reason = holidayVM.Reason;
                _holidayRepository.Update(holiday);
                _holidayRepository.Save();
                return RedirectToAction("ViewHolidays");
            }
            return View(holidayVM);
        }

        //public IActionResult UpdateProfile()
        //{
        //    return View();
        //}
        //public IActionResult UpdatePassword()
        //{
        //    return View();
        //}




    }
}
