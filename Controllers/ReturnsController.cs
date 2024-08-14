using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;

namespace ScannerWebAppUpdate.Controllers
{
    public class ReturnsController : Controller
    {
        private ScannerContext _context = new ScannerContext();

       public  ReturnsController()
        {
            _context.Database.EnsureCreated();
           // _context.ReturnParts.Load();

        }
        public async Task<IActionResult> Index()
        {
            List<ReturnPartsViewModel> rpvm = await _context.GetReturnPartsVM();



            ViewBag.PartsList = rpvm.OrderByDescending(part => part.ReturnPartId).ToList();

            return View();
        }

        public IActionResult CreateBarcode(ReturnPartsViewModel rpvm)
        {
            //Take View Model
            //Insert into ZPL Code
            //Show in view

            ViewBag.ZPLString = "\u0010CT~~CD,~CC^~CT~\r\n^XA\r\n~TA000\r\n~JSN\r\n^LT0\r\n^MNW\r\n^MTT\r\n^PON\r\n^PMN\r\n^LH0,0\r\n^JMA\r\n^PR2,2\r\n~SD15\r\n^JUS\r\n^LRN\r\n^CI27\r\n^PA0,1,1,0\r\n^XZ\r\n^XA\r\n^MMT\r\n^PW1800\r\n^LL1200\r\n^LS0\r\n^FT682,117^A0N,83,99^FH\\^CI28^FD" +
                "Return Part^FS^CI27\r\n^BY8,3,240^FT248,987^BCN,,Y,N,,A\r\n^FD" +
                rpvm.ReturnPartNumber+"^FS\r\n^FT79,365^A0N,83,112^FH\\^CI28^FD" +
                rpvm.PartNumber+"^FS^CI27\r\n^FT79,469^A0N,83,101^FH\\^CI28^FD" +
                rpvm.POName+"^FS^CI27\r\n^FT79,573^A0N,83,101^FH\\^CI28^FD" +
                rpvm.JobNumber+"^FS^CI27\r\n^PQ1,,,Y\r\n^XZ";

            return View();
        }

        public async Task<IActionResult> CheckIn(ReturnPart rpvm, int CheckedInQTY)
        {
            bool success = await _context.UpdateReturnPart(rpvm, CheckedInQTY);

            if (success)
            {
                Console.WriteLine(success);
            }
        
            return RedirectToAction("Index");
        }
    }
}
