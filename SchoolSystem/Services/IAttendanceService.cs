using SchoolSystem.Migrations;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public interface IAttendanceService
    {
        List<Attendance> GetAttendacesByStdId(string stdId, int month);
    }
}
