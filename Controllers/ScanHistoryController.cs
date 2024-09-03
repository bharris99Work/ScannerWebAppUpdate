using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ScannerWebAppUpdate.Models;

namespace ScannerWebAppUpdate.Controllers
{
    public class ScanHistoryController : Controller
    {
        private ScannerContext _context = new ScannerContext();
        private List<JobHistory> jobHistories = new List<JobHistory>();
        public ScanHistoryController()
        {
            _context.Database.EnsureCreated();
        }
        public async Task<IActionResult> Index()
        {

            

            ViewBag.PartHistoryList = await _context.GetJobHistory();
                
            return View();
        }

        public IActionResult DownloadFile(DateTime startDate, DateTime endDate, string itemNumber, string jobNumber)
        {
            Console.WriteLine(startDate +""+ endDate +""+ itemNumber);

            return RedirectToAction("Index");
        }

        public IActionResult PrintFile(DateTime startDate, DateTime endDate, string itemNumber, string jobNumber)
        {
            Console.WriteLine(startDate + "" + endDate + "" + itemNumber);
            return RedirectToAction("Index");
        }
    }
}
