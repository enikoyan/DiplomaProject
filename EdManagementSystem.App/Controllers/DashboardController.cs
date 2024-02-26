using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdManagementSystem.App.Controllers
{
    [Authorize(Roles = "teacher")]
    [Route("dashboard/[action]")]
    public class DashboardController : Controller
    {
        [ActionName("profile")]
        public IActionResult Profile()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            ViewBag.UserId = userId;
            return PartialView();
        }

        [ActionName("students")]
        public IActionResult Students()
        {
            return PartialView();
        }

        [ActionName("schedule")]
        public IActionResult Schedule()
        {
            return PartialView();
        }

        [ActionName("techSupport")]
        public IActionResult TechSupport()
        {
            return PartialView();
        }

        [ActionName("attendance")]
        public IActionResult Attendance()
        {
            return PartialView();
        }

        [ActionName("analytics")]
        public IActionResult Analytics()
        {
            return PartialView();
        }

        [ActionName("homeworks")]
        public IActionResult Homeworks()
        {
            return PartialView();
        }

        [ActionName("materials")]
        public IActionResult Materials()
        {
            return PartialView();
        }
    }
}
