using E_Commerce.Models;
using E_Commerce.Utility;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            if(roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.adminRole));
                await roleManager.CreateAsync(new(SD.companyRole));
                await roleManager.CreateAsync(new(SD.CustomerRole));
            }

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
                    // Ok
                    // Assign role to user
                    await userManager.AddToRoleAsync(applicationUser, SD.CustomerRole);
                    await signInManager.SignInAsync(applicationUser, false);
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
