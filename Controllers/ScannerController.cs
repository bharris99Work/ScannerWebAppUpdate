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

        public ScannerController() {

            _context.Database.EnsureCreated();
            _context.Parts.Load();

            PartsList = _context.Parts.Local.ToObservableCollection();
        }

        public IActionResult Index()
        {
            return View();
        }

  

        public void SeedDBParts()
        {
            List<Part> partList = new List<Part> { new Part("TestPart1", "TestJob1"), new Part("TestPart2", "TestJob1"), new Part("TestPart3", "TestJob1"), 
                new Part("TestPart4", "TestJob2"), new Part("TestPart5", "TestJob2") };

            _context.AddPartsList(partList);
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
            return View();
        }

        public void BeginSeeding()
        {
            SeedDBParts();
        }


        //Selects a Random Part to load (REMOVE DURING PRODUCTION)
        public IActionResult LoadRandomPart()
        {
            Random rand = new Random();

            int randomInd = rand.Next(0, PartsList.Count);

            return RedirectToAction("ScannedPart", PartsList[randomInd]);

        }
    }
}
