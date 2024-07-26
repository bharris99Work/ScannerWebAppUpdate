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

   
        public async Task<IActionResult> JobEditor(int jobId)
        {
     
            List<JobPartsViewModel> jobParts = await _context.JobPartsFind(jobId);

            List<JobPartsViewModel> assignedParts = jobParts.FindAll(jp => jp.AssignedParts > 0);

            ViewBag.JobParts = jobParts;
            ViewBag.AssignedParts = assignedParts;
            ViewBag.JobId = jobId;
            JobPartsPartialViewModel JPPV = await _context.GetJobPartPartialVM(jobId);

            Console.WriteLine(jobId);
            return View(JPPV);
        }

        public async Task<IActionResult> JobParts(int jobId)
        {
        

            JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobId);
            return PartialView("_JobParts", jppv);
        }

        [HttpPost]
        public async Task<IActionResult> AddPart(JobPartsViewModel jobPart)
        {
            try
            {
                bool success = await _context.AssignPart(jobPart, 1);

                if (success)
                {
                    JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobPart.JobId);
                    return PartialView("_JobParts", jppv);
                }
                return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });



            }
            catch (Exception ex) {
                return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePart(JobPartsViewModel jobPart)
        {
            try
            {
                bool success = await _context.UpdateJobPart(jobPart);

                if (success)
                {
                    JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobPart.JobId);
                    return PartialView("_JobParts", jppv);
                }
                return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });



            }
            catch (Exception ex)
            {
                return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemovePart(JobPartsViewModel jobPart)
        {
            try
            {

                bool success = await _context.RemovePart(jobPart, 1);

                if (success)
                {
                    JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobPart.JobId);
                    return PartialView("_JobParts", jppv);
                }
                return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });

            }
            catch (Exception ex)
            {
                return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });
            }
        }
    }
}
