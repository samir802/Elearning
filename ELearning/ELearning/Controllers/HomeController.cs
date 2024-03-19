using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace ELearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? date)
        {
            DateTime selectedDate;
            if (string.IsNullOrEmpty(date))
            {
                selectedDate = DateTime.Now;
            }
            else
            {
                selectedDate = DateTime.ParseExact(date, "yyyy-MM", CultureInfo.InvariantCulture);
            }

            var courses = new CourseModel();
            var topCourses = courses.GetTop3CoursesByEnrollment(selectedDate);
            ViewBag.SelectedDate = selectedDate;
            return View(topCourses);
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
        public IActionResult ProgressPage()
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
