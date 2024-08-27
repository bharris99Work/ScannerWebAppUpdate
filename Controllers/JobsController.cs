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
        private ObservableCollection<ReturnOption> ReturnOptionsList;




        public JobsController()
        {
            _context.Database.EnsureCreated();
            _context.ReturnOptions.Load();

            _context.Jobs.Load();

            ReturnOptionsList = _context.ReturnOptions.Local.ToObservableCollection();
            JobsList = _context.Jobs.Local.ToObservableCollection();
        }

        public IActionResult Index()
        {
            ViewBag.JobsList = JobsList;
            return View();
        }

      

   
        public async Task<IActionResult> JobEditor(Jobs selectedJob)
        {
     
            ViewBag.JobId = selectedJob.JobsId;
            ViewBag.JobName = selectedJob.JobNumber;
            ViewBag.ReturnOptions = ReturnOptionsList;
            JobPartsPartialViewModel JPPV = await _context.GetJobPartPartialVM(selectedJob.JobsId);

            Console.WriteLine(selectedJob.JobsId);
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

        //Check in parts for job
        public async Task<IActionResult> CheckInParts(int jobId)
        {
            try
            {

                CheckInPartsViewModel checkinparts = await _context.GetCheckInVM(jobId);

                ViewBag.JobId = jobId;



                return View(checkinparts);
            }

            catch (Exception ex)
            {

                return View();
            }
        }


        // Sends parts to database
        public async Task<IActionResult> UploadChecked(JobPartsViewModel checkedpart, int toCheckIn)
        {
            //Add to checked in

             bool success = await _context.CheckInPart(checkedpart.JobPartId, toCheckIn);

            //Return Partial view
            CheckInPartsViewModel checkinparts = await _context.GetCheckInVM(checkedpart.JobId);


            return PartialView("_CheckInPartsList", checkinparts);

        }





        //Check in parts list
        public async Task<IActionResult> CheckInPartsList(int jobId)
        {
            try
            {
                CheckInPartsViewModel checkinparts = await _context.GetCheckInVM(jobId);

                return PartialView("_CheckInPartsList", checkinparts);
            }

            catch (Exception ex)
            {
                return View();
            }

        }


        //Updates parts on job
        [HttpPost]
        public async Task<IActionResult> UpdatePart(JobPartsViewModel jobPart, string FunctionType, string ReturnReason, string ReturnNumber)
        {
            try
            {
                bool success = false;
                if (FunctionType.Trim() == "1")
                {
                    success = await _context.SignOffParts(jobPart);
                }
                else if (FunctionType.Trim() == "2")
                {
                    success = await _context.AddTruckPart(jobPart);
                }
                else if (FunctionType.Trim() == "3") 
                {
                    ReturnPart rp = new ReturnPart()
                    {
                        ReturnPartNumber = ReturnNumber,
                        JobPartId = jobPart.JobPartId,
                        QuantityReturned = jobPart.AssignedParts,
                        ReturnReason = ReturnReason
                    };

                    //Return Part
                    success = await _context.AddReturnPart(rp);
                }
                else if (FunctionType.Trim() == "4")
                {
                    success = await _context.UpdateJobPart(jobPart);
                }


                if (success)
                {
                        JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobPart.JobId);
                        jppv.uploadResult = "Successfully Updated Part";
                        return PartialView("_JobParts", jppv);
                 
                }
                else
                {
                    JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobPart.JobId);
                    jppv.uploadResult = "Failed To Update Part";
                    return PartialView("_JobParts", jppv);
                }
                //return View();
                //return RedirectToAction("JobEditor", new { jobId = jobPart.JobId });


            }
            catch (Exception ex)
            {

                JobPartsPartialViewModel jppv = await _context.GetJobPartPartialVM(jobPart.JobId);
                jppv.uploadResult = "Failed To Update Part";
                return PartialView("_JobParts", jppv);
                //return View();
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
