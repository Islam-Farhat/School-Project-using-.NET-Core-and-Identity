using SchoolSystem.Models;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Repository
{
    public interface IAdminRepository
    {
        Task<bool> AddTeacher(TeacherViewModel teacher);
        Task<bool> AddStudent(StudentViewModel student);  
        List<FeedbackVM> GetFeedbacks();
        bool AddFeedback(FeedbackVM feedback);

        bool UpdateStudent(StudentViewModel student);
        Task<StudentViewModel> GetStudentByID(StudentViewModel student);
        

    }
}
