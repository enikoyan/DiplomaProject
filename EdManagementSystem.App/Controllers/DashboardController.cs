using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.App.Controllers
{
    [Authorize(Roles = "teacher")]
    [Route("dashboard/[action]")]
    public class DashboardController : Controller
    {
        [ActionName("profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [ActionName("students")]
        public IActionResult Students()
        {
            return View();
        }

        [ActionName("schedule")]
        public IActionResult Schedule()
        {
            return View();
        }

        [ActionName("techSupport")]
        public IActionResult TechSupport()
        {
            return View();
        }

        [ActionName("attendance")]
        public IActionResult Attendance()
        {
            return View();
        }

        [ActionName("analytics")]
        public IActionResult Analytics()
        {
            return View();
        }

        [ActionName("homeworks")]
        public IActionResult Homeworks()
        {
            return View();
        }

        [ActionName("materials")]
        public IActionResult Materials()
        {
            return View();
        }
    }
}
