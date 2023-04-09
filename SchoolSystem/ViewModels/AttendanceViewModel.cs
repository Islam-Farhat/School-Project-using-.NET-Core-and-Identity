using SchoolSystem.Models;

namespace SchoolSystem.ViewModels
{
    public class AttendanceViewModel
    {
        //List<TableRowViewModel> tableRowViewModels { get; set; }
        public int id { get; set; }
    
        public AttendanceStatus AttendanceStatus { get; set; }
        public string userID_fk { get; set; }
        public int levelId { get; set; }
        public int ClassId { get; set; }

        public List<Classes> classes { get; set; }
        public List<ApplicationUser> students { get; set; }
        public List<Level> levels { get; set; }

    }
}
