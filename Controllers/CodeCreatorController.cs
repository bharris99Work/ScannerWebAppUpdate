using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ScannerWebAppUpdate.Controllers
{
    public class CodeCreatorController : Controller
    {
        private ScannerContext _context = new ScannerContext();
        private ObservableCollection<Part> PartsList;
     

        public CodeCreatorController()
        {

            _context.Database.EnsureCreated();
            _context.Parts.Load();
            PartsList = _context.Parts.Local.ToObservableCollection();


        }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {

            // Calculate the number of records to skip based on the current page and page size
            int skip = (page - 1) * pageSize;

            // Fetch the required parts from the database, ordered by Id, and apply pagination
            var parts = _context.Parts.OrderBy(p => p.PartId).Skip(skip).Take(pageSize).ToList();

            // Store the fetched parts in ViewBag to be used in the view
            ViewBag.PartsList = parts;

            // Set additional pagination information in ViewBag for use in the view
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(_context.Parts.Count() / (double)pageSize);

            ///Only grab first 20 parts from list initally,
            ///Setup next and previous buttons for going back and forth
            ///Next: Grabs next 20 parts.
            ///Previous: Grabs previous 20 parts.

            //ViewBag.PartsList = PartsList;
            return View();
        }

        public IActionResult CreatorResults(string image, string displayedPart)
        {
            ViewBag.QRCodeImageBase64 = image;
            ViewBag.DisplayedPart = displayedPart;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Part part)
        {
            // Handle the part data here
            // For example, you can save it to the database or perform other logic

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string dataStored = part.PartNumber;
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(dataStored, QRCodeGenerator.ECCLevel.Q);
            string image;

            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);
                string qrCodeImageBase64 = Convert.ToBase64String(qrCodeImage);
                //ViewBag.QRCodeImageBase64 = qrCodeImageBase64;
                image = qrCodeImageBase64;
            }


            // Redirect to a result page or back to the index
            return RedirectToAction("CreatorResults", new { image, displayedPart = part.PartNumber });
        }

        [HttpPost]
        public async Task<IActionResult> Search(string itemNumber, string description, string jobNumber)
        {
            var items = await _context.SearchPartAsync(itemNumber, jobNumber, description);
            
            ViewBag.PartsList = items;
            ViewBag.CurrentPage = 1;
            ViewBag.PageSize = items.Count;
            ViewBag.TotalPages = 1;

            return View("Index");
        }


    }
}
