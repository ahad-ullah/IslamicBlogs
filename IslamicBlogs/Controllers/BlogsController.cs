using Microsoft.AspNetCore.Mvc;

namespace IslamicBlogs.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
