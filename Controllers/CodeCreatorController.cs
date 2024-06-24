using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
        public IActionResult Index()
        {
            ViewBag.PartsList = PartsList;
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
            string dataStored = part.ItemNumber;
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
            return RedirectToAction("CreatorResults", new { image, displayedPart = part.ItemNumber });
        }


    }
}
