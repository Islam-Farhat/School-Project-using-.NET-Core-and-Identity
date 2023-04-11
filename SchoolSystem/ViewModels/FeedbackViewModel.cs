using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SchoolSystem.ViewModels
{
    [Keyless]
    public class FeedbackViewModel
    {
        [DisplayName("FeedBack")]
        public string FeedbackTxt { get; set; }
    }
}
