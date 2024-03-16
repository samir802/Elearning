using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ELearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult StudentPage()
        {
            return View();
        }

        public IActionResult CoursePage()
        {
            return View();
        }

        public IActionResult InstructorPage()
        {
            return View();
        }
        public IActionResult LessonPage()
        {
            return View();
        }
         public IActionResult EnrollmentPage()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
