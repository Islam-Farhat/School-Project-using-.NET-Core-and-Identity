using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet]
        public IActionResult Register()

        {
           
            return View();
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.UserName, Email = model.Email,Name=model.Name,PhoneNumber=model.PhoneNumber,Address = model.Address,Gender= (Models.Gender)model.Gender,BirthDate=model.BirthDate ,photoUrl=model.photoUrl};
              
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Student");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                
                }
            }

            return View(model);
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
                    _signInManager.PasswordSignInAsync(user, loginVM
                        .Password, loginVM.RememberMe, false);
                    //if(user.)
                    //return RedirectToAction("Index", "Student");
                    //return RedirectToAction("Index", "Student");
                    //return RedirectToAction("Index", "Student");
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
