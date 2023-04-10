using App.Repos;
using SchoolSystem.Migrations;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class AttendanceService:IAttendanceService
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        public AttendanceService(IRepository<Attendance> attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public List<Attendance> GetAttendacesByStdId(string stdId, int month)
        {
            return _attendanceRepository.GetAll().Where(a=>a.userID_fk==stdId && a.Date.Month==month && a.Date.Year==DateTime.Now.Year).ToList();


        }

    }
}
