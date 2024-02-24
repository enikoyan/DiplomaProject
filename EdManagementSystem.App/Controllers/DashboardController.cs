using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.App.Controllers
{
    [Authorize (Roles = "teacher")]
    [Route("dashboard/[action]")]
    public class DashboardController : Controller
    {
        [ActionName("profile")]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
