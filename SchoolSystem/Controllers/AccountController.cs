using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Identity.Client;
using SchoolSystem.Models;
using SchoolSystem.ViewModels;
using System.Security.Claims;
using System.Security.Principal;

namespace SchoolSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            if (ModelState.IsValid)
            {
              ApplicationUser user = await  _userManager.FindByNameAsync(loginVM.UserName);
                if(user != null) {

                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                    if (!result.Succeeded)
                    {

                        return RedirectToAction("Index", "Home");
                    }
            
   
                    if (await _userManager.IsInRoleAsync(user,"Admin"))
                        return RedirectToAction("StudentsReports", "Teacher");
                    else if (await _userManager.IsInRoleAsync(user, "Student"))
                        return RedirectToAction("Index", "Student");
                    else if (await _userManager.IsInRoleAsync(user, "Teacher"))
                        return RedirectToAction("Index", "Teacher");
                    else
                    {
                        ModelState.AddModelError("", "Not Authenticated");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password Wrong");
                    
                }
            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
