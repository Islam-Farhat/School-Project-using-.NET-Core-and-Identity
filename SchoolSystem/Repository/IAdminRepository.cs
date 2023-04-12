using SchoolSystem.Models;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Repository
{
    public interface IAdminRepository
    {
        Task<bool> AddTeacher(TeacherViewModel teacher);
        Task<bool> AddStudent(StudentViewModel student);
<<<<<<< HEAD
  
        List<FeedbackVM> GetFeedbacks();
        bool AddFeedback(FeedbackVM feedback);
        
       
=======
        bool UpdateStudent(StudentViewModel student);
        Task<StudentViewModel> GetStudentByID(StudentViewModel student);
        
        List<FeedbackVM> GetFeedbacks();
        bool AddFeedback(FeedbackVM feedback);

        //Task<List<Classes>> GetClasses();
        //Classes GetClassByID(int? id);
        //bool UpdateClass(ClassViewModel clas);
        //bool DeleteClass(int? id);
        //Task<bool> AddClass(ClassViewModel clas);

        //Task<List<Level>> GetLevels();
        // Task<bool> AddLevel(LevelViewModel level);
        // bool DeleteLevel(int? id);
        //bool UpdateLevel(LevelViewModel level);
        //string GetLevelByID(int? id);
>>>>>>> d4587ef25cb673419e23722cb7c1a6dbbfe5ad5c
    }
}
