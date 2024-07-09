using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;

namespace ScannerWebAppUpdate.Controllers
{
    public class PartDeliveriesController : Controller
    {
        private ScannerContext _context = new ScannerContext();

        public PartDeliveriesController() {
          
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
