using IslamicBlogs.Models;
using IslamicBlogs.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {

                    var isLogedIn = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                    if (isLogedIn.Succeeded)
                    {
                        HttpContext.Session.SetString("UserEmail", user.Email);
                        HttpContext.Session.SetString("UserName", user.UserName);
                        return RedirectToAction("Index", "Blogs");
                    }
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
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    if (model.Password == model.ConfirmPassword)
                    {
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
                        ViewBag.ErrorMessage = result.Errors.ToString();
                    }
                    ViewBag.ErrorMessage = "Passowrds does not match";
                }
                ViewBag.ErrorMessage = "User with same email already exist";
            }
            return View();
        }
    }
}
