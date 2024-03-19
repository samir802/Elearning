// DashboardController.cs
using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using ELearning.Models;

namespace ELearning.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string _connectionString = "User Id=elearningdatabase;Password=elearningdatabase;Data Source=localhost:1521/orcl;";

        public IActionResult Index()
        {
            //DashboardModel dashboardData = new DashboardModel();
            //dashboardData.PopulateDashboardData(_connectionString);

            //ViewBag.dashboardData = dashboardData;
            //return View();
           DashboardModel dashboardData = new DashboardModel();
            dashboardData.PopulateDashboardData(_connectionString);

            return View(dashboardData);
        }
    }
}
