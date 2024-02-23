using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.App.Controllers
{
    public class AuthController : Controller
    {
        [Route("[controller]/login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("[controller]/recovery-password")]
        public IActionResult RecoveryPassword()
        {
            return View();
        }
    }
}
