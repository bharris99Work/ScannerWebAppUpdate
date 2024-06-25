using Microsoft.AspNetCore.Mvc;

namespace ScannerWebAppUpdate.Controllers
{
    public class TestPagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
