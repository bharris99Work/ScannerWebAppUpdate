using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;

namespace ScannerWebAppUpdate.Controllers
{
    public class ScanHistoryController : Controller
    {
        private ScannerContext _context = new ScannerContext();

        public ScanHistoryController()
        {
            _context.Database.EnsureCreated();
            _context.Parts.Load();
           // _context.PartHistory.Load();
            //PartsList = _context.Parts.Local.ToObservableCollection();
        }
        public IActionResult Index()
        {/*
            var partHistoryViemModels = _context.PartHistory
                .Join(_context.Parts,
                partHistory => partHistory.PartId,
                part => part.PartId,
                (partHistory, part) => new PartHistoryPartViewModel
                {
                    Part = part,
                    PartHistory = partHistory
                }).ToList();*/

            ViewBag.PartHistoryList = null;
                
                //partHistoryViemModels;

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
