using EdManagementSystem.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Security.Claims;

namespace EdManagementSystem.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Role = this.User.FindFirst(ClaimTypes.Role).Value;
            return View();
        }
    }
}
