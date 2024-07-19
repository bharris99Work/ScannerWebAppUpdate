using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;

namespace ScannerWebAppUpdate.Controllers
{
    public class ScannerController : Controller
    {
        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<Part> PartsList;
        private ObservableCollection<ReturnOption> ReturnOptionsList;

        public ScannerController() {

            _context.Database.EnsureCreated();
            _context.Parts.Load();
            _context.ReturnOptions.Load();

            PartsList = _context.Parts.Local.ToObservableCollection();
            ReturnOptionsList = _context.ReturnOptions.Local.ToObservableCollection();
        }

        public IActionResult Index(string result)
        {
            ResultPart? resultPart = new ResultPart();
            if (result != null) {
               resultPart = JsonConvert.DeserializeObject<ResultPart>(result);
                ViewBag.PartFound = resultPart.Result;
                ViewBag.ScannedText = resultPart.ScannedString;
            }
      
            return View();
        }

     


        [HttpPost]
        public async Task<IActionResult> ProcessScannedPart(string scannedPart)
        {
            ResultPart resultPart = new ResultPart();
            Part matchingPart = null;

            if (scannedPart != null && scannedPart != string.Empty) {
                // Simulate an asynchronous database search
                 matchingPart = await Task.Run(() =>
                    PartsList.FirstOrDefault(part =>
                        part.PartNumber.ToLower().Trim() == scannedPart.ToLower().Trim()
                    )
                );
            }
          

            if (matchingPart != null)
            {
                return RedirectToAction("ScannedPart", matchingPart);
            }
       
            string newresult = "Part Not Found";
            resultPart.Result = newresult;
            resultPart.ScannedString = scannedPart;

            string serializedResult = JsonConvert.SerializeObject(resultPart);

            return RedirectToAction("Index", new { result = serializedResult });
        }

        public IActionResult ScannedPart(Part scannedPart, string updatedPartJson)
        {
            //PartUpdate? updatedPart = null;

            //if(updatedPartJson != null && updatedPartJson != "")
            //{
            //    updatedPart = JsonConvert.DeserializeObject<PartUpdate>(updatedPartJson);

            //}


            //if(updatedPart == null || updatedPart.part == null)
            //{
            //    ViewBag.ScannedPart = scannedPart;
            //    ViewBag.ReturnOptions = ReturnOptionsList;
            //    ViewBag.TechOptions = TechOptionsList;
            //    return View();
            //}
            //else
            //{
            //    ViewBag.ScannedPart = updatedPart.part;
            //    ViewBag.ReturnOptions = ReturnOptionsList;
            //    ViewBag.TechOptions = TechOptionsList;

            //    if (updatedPart.updated)
            //    {
            //        ViewBag.UploadStatus = "Successfully Updated: " + updatedPart.part.ItemNumber;
            //    }
            //    else
            //    {
            //        ViewBag.UploadStatus = "Failed to Update: " + updatedPart.part.ItemNumber;
            //    }
                return View();
           // }
  
        }

     


        //Selects a Random Part to load (REMOVE DURING PRODUCTION)
        public IActionResult LoadRandomPart()
        {
            try
            {
                Random rand = new Random();

                int randomInd = rand.Next(0, PartsList.Count);

                return RedirectToAction("ScannedPart", PartsList[randomInd]);
            }
            catch (Exception ex) {
                return RedirectToAction("Index");
            }
          

        }

        [HttpPost]
        public async Task<IActionResult> UploadPart(Part newPart, string returnOther, string techOther)
        {
            ////Other string and no option selected
            //if (techOther != null && techOther.Length > 0) {
            //    newPart.TechOption = techOther;
            //    Console.WriteLine("Tech Other string was selected");

            //}
            ////Option selected, no other string
            //else if (newPart.TechOption != null && newPart.TechOption.Length > 0) {

            //    Console.WriteLine("Tech Option was selected");

            //}
            ////No Option selected
            //else {
            //    newPart.TechOption = "";
            //    Console.WriteLine("No Option was selected");
            //}



            ////Other string and no option selected
            //if (returnOther != null && returnOther.Length > 0) {

            //    newPart.ReturnOption = returnOther;
            //    Console.WriteLine("Return Other string was selected");

            //}
            ////Option selected, no other string
            //else if (newPart.ReturnOption != null && newPart.ReturnOption.Length > 0) {

            //    Console.WriteLine("Return Option was selected");
            //}
            ////No Option selected
            //else {
            //    newPart.ReturnOption = "";
            //    Console.WriteLine("No Option was selected");

            //}
            // PartUpdate partUpdate = new PartUpdate(newPart, await _context.UpdatePart(newPart));
            // string serializedPartUpdate = JsonConvert.SerializeObject(partUpdate);


            //return RedirectToAction("ScannedPart", new { updatedPartJson = serializedPartUpdate });
            return RedirectToAction("Index");
        }



        public class ResultPart
        {
            public string Result { get; set; }
            public string ScannedString { get; set; }

            public ResultPart() { }
            public ResultPart(string result, string scannedPart)
            {
                Result= result;
                ScannedString = scannedPart;
            }
        }
    }
}
