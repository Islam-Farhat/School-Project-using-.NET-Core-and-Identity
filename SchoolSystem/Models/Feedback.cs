using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string FeedbackText { get; set; }
        public string Response { get; set;}
        [ForeignKey("ApplicationUser")]
        public string userID_fk { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
