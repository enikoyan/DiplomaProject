using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdManagementSystem.App.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        [Route("admin-panel")]
        public IActionResult AdminPanel()
        {
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)!.Value;
            return View();
        }
    }
}
