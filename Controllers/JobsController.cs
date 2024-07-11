using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;

namespace ScannerWebAppUpdate.Controllers
{
    public class JobsController : Controller
    {
        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<Jobs> jobs;

        public JobsController()
        {
            _context.Database.EnsureCreated();
            _context.Jobs.Load();

            jobs = _context.Jobs.Local.ToObservableCollection();
        }

        public IActionResult Index()
        {
            ViewBag.JobsList = jobs;
            return View();
        }

        public IActionResult CurrentParts()
        {
            return View();
        }

        public IActionResult ReturnQueue()
        {
            return View();
        }

        public IActionResult WarehouseParts()
        {
            return View();
        }

        [HttpPost]
        public IActionResult JobEditor(Jobs job)
        {
            //Job details
            //Parts Associated With That Job
            //List of parts in truck/warehouse for picking - these parts have qr code for scanning
            //

            ViewBag.SelectedJob = job;


            Console.WriteLine(job);
            return View();
        }



    }
}
