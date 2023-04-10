using Microsoft.AspNetCore.Identity;
using SchoolSystem.Models;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Repository
{
    public class AdminRepository : IAdminRepository
    {
        SchoolDB context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AdminRepository(SchoolDB _context, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)//inject context from Program.cs
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        public async Task<bool> AddTeacher(TeacherViewModel teacherVM)
        {
            ApplicationUser teacher = new ApplicationUser();
            teacher.Name = teacherVM.Name;
            teacher.Email = teacherVM.Email;
            teacher.UserName = teacherVM.UserName;
            teacher.Address = teacherVM.Address;
            teacher.PhoneNumber = teacherVM.Phone;
            teacher.photoUrl = teacherVM.Photo.FileName;
            teacher.BirthDate = teacherVM.BirthDate;
            teacher.Gender = (Models.Gender)teacherVM.Gender;
            teacher.PasswordHash = teacherVM.Password;

            IdentityResult result = await userManager.CreateAsync(teacher, teacher.PasswordHash);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(teacher, "Admin");
                await signInManager.SignInAsync(teacher, isPersistent: false);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> AddStudent(StudentViewModel studentVM)
        {
            ApplicationUser student = new ApplicationUser();
            student.Name = studentVM.Name;
            student.Email = studentVM.Email;
            student.UserName = studentVM.UserName;
            student.Address = studentVM.Address;
            student.PhoneNumber = studentVM.Phone;
            student.photoUrl = studentVM.Photo.FileName;
            student.BirthDate = studentVM.BirthDate;
            student.Gender = (Models.Gender)studentVM.Gender;
            student.PasswordHash = studentVM.Password;
            student.levelID_fk = studentVM.levelID_fk;
            student.classID_fk = studentVM.classID_fk;

            IdentityResult result = await userManager.CreateAsync(student, student.PasswordHash);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(student, "Student");
                await signInManager.SignInAsync(student, isPersistent: false);
                return true;
            }
            else
                return false;
        }

        //I will replace them when we create level repo and class repo
        public async Task<List<Classes>> GetClasses()
        {
            return context.Classes.ToList();
        }
        public async Task<List<Level>> GetLevels()
        {
            return context.Levels.ToList();
        }
    }
}
