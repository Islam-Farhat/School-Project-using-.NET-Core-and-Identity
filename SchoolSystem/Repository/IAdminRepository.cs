using SchoolSystem.Models;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Repository
{
    public interface IAdminRepository
    {
        Task<bool> AddTeacher(TeacherViewModel teacher);
        Task<bool> AddStudent(StudentViewModel student);
        Task<List<Classes>> GetClasses();
        Task<List<Level>> GetLevels();
    }
}
