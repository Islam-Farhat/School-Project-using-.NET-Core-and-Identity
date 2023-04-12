using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Identity.Client;
using SchoolSystem.Models;
using SchoolSystem.ViewModels;

namespace SchoolSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

     
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
              ApplicationUser user =await  _userManager.FindByNameAsync(loginVM.UserName);
                if(user != null) { }
                else
                {
                    ModelState.AddModelError("", "Username or Password Wrong");
                    _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                    if (User.IsInRole("Admin"))
                        return RedirectToAction("Index", "Admin");
                    else if (User.IsInRole("Student"))
                        return RedirectToAction("Index", "Student");
                    else if (User.IsInRole("Teacher"))
                        return RedirectToAction("Index", "Teacher");
                    else
                    {
                        //UnAuthorizedPage
                        //ModelState.AddModelError("", "");
                    }
                }
            }

            return View(loginVM);
        }


        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
