using App.Repos;
using SchoolSystem.Models;

namespace SchoolSystem.Repository
{
    public class TeacherRepo : GenericRepository<ApplicationUser> , ITeacherRepo 
    {
        public TeacherRepo(SchoolDB context) : base(context)
        {
            
        }


    }
}
