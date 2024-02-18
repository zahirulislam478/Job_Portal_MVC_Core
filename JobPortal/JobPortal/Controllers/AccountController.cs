using JobPortal.Models;
using JobPortal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace JobPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly JobPortalDbContext db;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, JobPortalDbContext db)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               var user= await userManager.FindByNameAsync(model.UserName);
                
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var result = await signInManager.PasswordSignInAsync(model.UserName,
                           model.Password, false, lockoutOnFailure: true);
                    if(result.Succeeded)
                    {
                        if (await userManager.IsInRoleAsync(user, "JobPoster"))
                        {
                            return RedirectToAction("Index", "JobPosters");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");


                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");


                }
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.IsJobPoster)
                    {
                      await   userManager.AddToRoleAsync(user, "JobPoster");
                        db.JobPosters.Add(new JobPoster { UserId = user.Id });
                        db.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "JobSeeker");
                        db.JobSeekers.Add(new JobSeeker { UserId = user.Id });
                        db.SaveChanges();
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
