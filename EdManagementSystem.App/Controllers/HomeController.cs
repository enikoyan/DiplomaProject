using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return Redirect("/admin-panel");
            }
            else return Redirect("/dashboard/profile");
        }
    }
}
