// DashboardController.cs
using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using ELearning.Models;

namespace ELearning.Controllers
{
    public class DashboardController : Controller
    {
        string conString = DbConnection.conString;

        public IActionResult Index()
        {
            //DashboardModel dashboardData = new DashboardModel();
            //dashboardData.PopulateDashboardData(_connectionString);

            //ViewBag.dashboardData = dashboardData;
            //return View();
           DashboardModel dashboardData = new DashboardModel();
            dashboardData.PopulateDashboardData(conString);

            return View(dashboardData);
        }
    }
}
