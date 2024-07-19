using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;

namespace ScannerWebAppUpdate.Controllers
{
    public class JobsController : Controller
    {
        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<Jobs> JobsList;
        private ObservableCollection<Part> PartsList;
        private List<Part> JobPartsList;


        public JobsController()
        {
            _context.Database.EnsureCreated();
            _context.Jobs.Load();
            _context.Parts.Load();
            //_context.AssignedParts.Load();


            PartsList = _context.Parts.Local.ToObservableCollection();
            JobsList = _context.Jobs.Local.ToObservableCollection();
        }

        public IActionResult Index()
        {
            ViewBag.JobsList = JobsList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JobParts(int jobId)
        {

           // JobPartsList = await _context.JobPartsFind(jobId);
            var PartsOnTruck = await _context.GetTruckParts("TestTruck");
            ViewBag.JobID = jobId;  
            return PartialView("_JobParts", PartsOnTruck);
        }

        public IActionResult Home()
        {
            return PartialView("_Home");
        }

        public IActionResult AddPart()
        {
            return PartialView("_AddPart");
        }

        [HttpPost]
        public async Task<IActionResult> JobEditor(Jobs job)
        {
            //Job details
            //ViewBag.JobParts = await _context.JobPartsFind(job);

          
            //Removing part from truck

            Console.WriteLine(job);
            return View();
        }

        [HttpPost]
        public IActionResult AddPart(Part part, int AssignedQuanity, int jobId)
        {
            try
            {
                //User clicks add
                //Dialog asking for quantity opens
                //User hits submit
                //Informaiton is sent


               //AddPart Dialog:
               //Add All Option
               //Add One by default
               //Add Multiple

                //Assignments:
                //Part.Quantity -= Assigned Quantity
                //AssignedPart.Quantity = Assigned Quantity
                //AssignedPart.JobId = JobId
                //AssignedPart.Status = Idle
                //ReturnReason = null

                //Update Part
                //Update AssignedPart
                //Save Changes
                return View();

            }
            catch (Exception ex) {

                return View();
            }
        }

    }
}
