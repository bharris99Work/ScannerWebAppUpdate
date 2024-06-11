using Microsoft.AspNetCore.Mvc;

namespace ScannerWebAppUpdate.Controllers
{
    public class CodeCreatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatorResults()
        {
            return View();
        }


    }
}
