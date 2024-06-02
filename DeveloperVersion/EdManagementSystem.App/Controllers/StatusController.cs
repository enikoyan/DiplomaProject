using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.App.Controllers
{
    [AllowAnonymous]
    [Route("status")]
    public class StatusController : Controller
    {
        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("not-found")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
