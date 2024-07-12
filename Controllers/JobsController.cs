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
            _context.AssignedParts.Load();


            PartsList = _context.Parts.Local.ToObservableCollection();
            JobsList = _context.Jobs.Local.ToObservableCollection();
        }

        public IActionResult Index()
        {
            ViewBag.JobsList = JobsList;
            return View();
        }

        public async Task<IActionResult> JobParts(int jobId)
        {

            JobPartsList = await _context.JobPartsFind(jobId);
            
            return PartialView("_JobParts", JobPartsList);
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
        public IActionResult JobEditor(Jobs job)
        {
            //Job details
            ViewBag.SelectedJob = job;
            ViewBag.SelectedJobId = job.JobsId;


            Console.WriteLine(job);
            return View();
        }

    }
}
