using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Models
{
    public class Holiday
    {
        public int  Id { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        [ForeignKey("ApplicationUser")]
        public string userID_fk { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
