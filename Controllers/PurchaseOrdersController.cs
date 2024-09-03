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
        private ObservableCollection<Part> PartsList;


        public PurchaseOrdersController()
        {
            _context.Database.EnsureCreated();
            _context.Jobs.Load();
            _context.Trucks.Load();
            _context.Parts.Load();

            JobsList = _context.Jobs.Local.ToObservableCollection();
            TrucksList = _context.Trucks.Local.ToObservableCollection();
            PartsList = _context.Parts.Local.ToObservableCollection();
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
            ViewBag.allparts = PartsList;
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




        public async Task<IActionResult> UploadPurchaseOrder([FromBody] PurchaseOrderUpload purchaseOrderViewModel)
        {
            bool success = false;

            //Stock Order
            if (purchaseOrderViewModel.SelectedJob == "None" && purchaseOrderViewModel.SelectedTruck != "None")
            {
                success = await _context.UploadStockOrder(purchaseOrderViewModel.parts, purchaseOrderViewModel.POName, purchaseOrderViewModel.SelectedTruck);

            }
            //Job Order
            else if (purchaseOrderViewModel.SelectedJob != "None" && purchaseOrderViewModel.SelectedTruck == "None")
            {
                success = await _context.UploadJobOrder(purchaseOrderViewModel.parts, purchaseOrderViewModel.POName, purchaseOrderViewModel.SelectedJob);

            }
            //Pick List
            else if (purchaseOrderViewModel.SelectedJob != "None" && purchaseOrderViewModel.SelectedTruck != "None")
            {
                success = await _context.UploadPickList(purchaseOrderViewModel.parts, purchaseOrderViewModel.POName, purchaseOrderViewModel.SelectedTruck);

            }

            if (success) {
                return RedirectToAction("Index");

            }


            return RedirectToAction("Index");
        }



      
    }
}
