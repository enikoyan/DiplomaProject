using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdManagementSystem.App.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("admin-panel")]
    public class AdminController : Controller
    {
        public IActionResult AdminPanel()
        {
            ViewBag.Role = User.FindFirst(ClaimTypes.Role)!.Value;
            return View();
        }
    }
}
