using Microsoft.AspNetCore.Mvc;

namespace ScannerWebAppUpdate.Controllers
{
    public class ScannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ScannedPart()
        {
            return View();
        }
    }
}
