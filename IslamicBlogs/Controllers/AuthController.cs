using IslamicBlogs.Models;
using IslamicBlogs.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IslamicBlogs.Controllers
{
    public class AuthController : Controller
    {
        private readonly BlogsDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthController(BlogsDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var signInResult = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        HttpContext.Session.SetString("UserEmail", user.Email);
                        HttpContext.Session.SetString("UserName", user.UserName);
                    }
                    return RedirectToAction("Index", "Blogs");
                }

                ViewBag.ErrorMessage = "Invalid Login Attempt";
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", ex.Message);
            }
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
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "User with same email already exist";
                    return View();
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.ErrorMessage = "Passwords do not match";
                    return View();
                }

                var newUser = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                ViewBag.ErrorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
            }

            return View();
        }
    }
}
