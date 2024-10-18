using E_Commerce.Models;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUserVM userVm)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = userVm.Name,
                    Email = userVm.Email,
                    City = userVm.City
                };

                var result = await userManager.CreateAsync(applicationUser, userVm.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Password", "Invalid Password");
            }

            return View(userVm);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM userVm)
        {
            if (ModelState.IsValid)
            {
                var userDb = await userManager.FindByNameAsync(userVm.User);

                if(userDb != null)
                {
                    var finalResult = await userManager.CheckPasswordAsync(userDb, userVm.Password);

                    if (finalResult)
                    {
                        // Login ==> Create ID (cookies)
                        await signInManager.SignInAsync(userDb, userVm.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        // Error in password
                        ModelState.AddModelError("Password", "invalid passwrod");
                }
                else
                    // Error in userName
                    ModelState.AddModelError("User", "invalid UserName");

            }
            return View(userVm);
        }

        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
