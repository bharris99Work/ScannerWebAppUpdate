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

        public IActionResult Index(string result)
        {
           ViewBag.PartFound = result;
            return View();
        }

     


        [HttpPost]
        public IActionResult ProcessScannedPart(string scannedPart)
        {
            foreach (var part in PartsList)
            {
                if (part.ItemNumber == scannedPart)
                {
                    return RedirectToAction("ScannedPart", part);
                }
            }
            string newresult = "Part Not Found";
            return RedirectToAction("Index", new { result = newresult });
        }

        public IActionResult ScannedPart(Part scannedPart, string uploadStat)
        {
            ViewBag.ScannedPart = scannedPart;
            ViewBag.ReturnOptions = ReturnOptionsList;
            ViewBag.TechOptions = TechOptionsList;
            ViewBag.UploadStatus = uploadStat;
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
        public IActionResult UploadPart(Part newPart, string returnOther, string techOther)
        {
            //Other string and no option selected
            if (techOther != null && techOther.Length > 0) {
                newPart.TechOption = techOther;
                Console.WriteLine("Tech Other string was selected");
            
            }
            //Option selected, no other string
            else if (newPart.TechOption != null && newPart.TechOption.Length > 0) {

                Console.WriteLine("Tech Option was selected");

            }
            //No Option selected
            else {
                newPart.TechOption = "";
                Console.WriteLine("No Option was selected");
            }



            //Other string and no option selected
            if (returnOther != null && returnOther.Length > 0) {

                newPart.ReturnOption = returnOther;
                Console.WriteLine("Return Other string was selected");

            }
            //Option selected, no other string
            else if (newPart.ReturnOption != null && newPart.ReturnOption.Length > 0) {

                Console.WriteLine("Return Option was selected");
            }
            //No Option selected
            else {
                newPart.ReturnOption = "";
                Console.WriteLine("No Option was selected");

            }

            string uploadStatus = "";
            bool uploaded = _context.UpdatePart(newPart);
            if (uploaded)
            {
                //Display update succesful message
                uploadStatus = "Update Succesful";
            }
            else
            {
                //Display update failed
                uploadStatus = "Update Failed";
            }

            return RedirectToAction("ScannedPart",new { uploadStat = uploadStatus } );
        }
    }
}
