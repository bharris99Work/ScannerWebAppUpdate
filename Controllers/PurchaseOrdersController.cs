using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;

namespace ScannerWebAppUpdate.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<Jobs> JobsList;
        private ObservableCollection<Truck> TrucksList;

        public PurchaseOrdersController()
        {
            _context.Database.EnsureCreated();
            _context.Jobs.Load();
            _context.Trucks.Load();

            JobsList = _context.Jobs.Local.ToObservableCollection();
            TrucksList = _context.Trucks.Local.ToObservableCollection();
        }

        public async Task<IActionResult> Index()
        {
            List<PurchaseOrderViewModel> purchaseOrders = await _context.GetPOViewModels();
            ViewBag.PurchaseOrders = purchaseOrders;
            return View();
        }

        public IActionResult CreatePO()
        {
            List<string> poOptions = new List<string>()
            {
                "JobOrder",
                "StockOrder",
                "PickList"
            };

            ViewBag.jobs = JobsList;
            ViewBag.trucks = TrucksList;
            ViewBag.potypes = poOptions;
            return View();
        }

        public async Task<IActionResult> EditPO(PurchaseOrderViewModel poVM)
        {
            ViewBag.POName = poVM.POName;
            
            ViewBag.TruckName = poVM.TruckName;

            ViewBag.JobName = poVM.JobName;

            ViewBag.Type = poVM.Type;

            //Grab all parts with that PO
            ViewBag.POParts = await _context.GetPoParts(poVM.PurchaseOrderID);

            return View();


        }

      

        public async Task<IActionResult> UploadPo(string poNumber, int partsSelected, string selectedJob, string selectedTruck) 
        {
            bool success = false;

            //Stock Order
            if (selectedJob == null && selectedTruck != string.Empty) {

                //Create Stock Order:

                //Save PO
                PurchaseOrder newPo = new PurchaseOrder()
                {
                    Name = poNumber,
                    TruckId = int.Parse(selectedTruck.Trim()),
                    Type = "Stock Order"
                };

                success = await _context.AddPurchaseOrder(newPo);

                if (success) {

                   success = await _context.CreatePOParts(partsSelected, poNumber);
                }
                if (success) {

                    success = await _context.AddTruckParts(poNumber, int.Parse(selectedTruck.Trim()));
                
                }


                //Add List of parts to truckparts
            
            }

            //Job Order
           else if (selectedJob != string.Empty && selectedTruck == string.Empty)
            {


            }
            //Pick List
           else if (selectedJob != string.Empty && selectedTruck != string.Empty)
            {


            }

            PurchaseOrder newPO = new PurchaseOrder()
            {
                Name = poNumber,
            };

            return RedirectToAction("Index");
        }
    }
}
