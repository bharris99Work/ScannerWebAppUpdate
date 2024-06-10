using Microsoft.AspNetCore.Mvc;

namespace ScannerWebAppUpdate.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
