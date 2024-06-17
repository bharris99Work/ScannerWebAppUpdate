using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;

namespace ScannerWebAppUpdate.Controllers
{
    public class ScannerController : Controller
    {
        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<Part> PartsList;
        private ObservableCollection<TechOption> TechOptionsList;
        private ObservableCollection<ReturnOption> ReturnOptionsList;

        public ScannerController() {

            _context.Database.EnsureCreated();
            _context.Parts.Load();
            _context.TechOptions.Load();
            _context.ReturnOptions.Load();

            PartsList = _context.Parts.Local.ToObservableCollection();
            TechOptionsList = _context.TechOptions.Local.ToObservableCollection();
            ReturnOptionsList = _context.ReturnOptions.Local.ToObservableCollection();
        }

        public IActionResult Index()
        {
            return View();
        }

  
        [HttpPost]
        public IActionResult ProcessScannedPart(string scannedPart)
        {
            foreach(var part in PartsList)
            {
                if(part.ItemNumber == scannedPart)
                {
                    return RedirectToAction("ScannedPart", part);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult ScannedPart(Part scannedPart)
        {
            ViewBag.ScannedPart = scannedPart;
            ViewBag.ReturnOptions = ReturnOptionsList;
            ViewBag.TechOptions = TechOptionsList;
            return View();
        }


        //Selects a Random Part to load (REMOVE DURING PRODUCTION)
        public IActionResult LoadRandomPart()
        {
            Random rand = new Random();

            int randomInd = rand.Next(0, PartsList.Count);

            return RedirectToAction("ScannedPart", PartsList[randomInd]);

        }

        [HttpPost]
        public IActionResult UploadPart(Part newPart, ReturnOption returnOpt, TechOption techOpt, string returnOther, string techOther) {
            Console.Write("This is new part");

            return RedirectToAction("Index");
        }
    }
}
