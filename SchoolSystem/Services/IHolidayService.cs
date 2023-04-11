using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public interface IHolidayService
    {
        List<Holiday> GetAllHolidays();


         Holiday GetHolidayById(int id);


        void updateHoliday(Holiday holiday);

        void Save();
        


    }
}
