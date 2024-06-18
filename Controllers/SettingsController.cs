using Microsoft.AspNetCore.Mvc;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace ScannerWebAppUpdate.Controllers
{
    public class SettingsController : Controller
    {


        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<ReturnOption>? TestReturns;
        private ObservableCollection<TechOption>? TestTechs;
        private ObservableCollection<Part>? TestParts;


        public SettingsController()
        {
            _context.Database.EnsureCreated();
        }
        public IActionResult Index(string uploadStat)
        {
            ViewBag.UploadStatus = uploadStat;
            return View();
        }

        public IActionResult UploadTech(string techDescription)
        {
            try
            {
                if (techDescription != null & techDescription != string.Empty)
                {
                    TechOption techOption = new TechOption(techDescription);
                    bool updateStatus = _context.AddTech(techOption);
                    return uploadStatus(updateStatus);
                }
                return uploadStatus(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return uploadStatus(false);
            }
        }

        public IActionResult UploadReturn(string returnDescription)
        {
            try
            {

                if (returnDescription != null & returnDescription != string.Empty)
                {
                    ReturnOption returnOption = new ReturnOption(returnDescription);
                    bool updateStatus = _context.AddReturn(returnOption);
                    return uploadStatus(updateStatus);

                }
                return uploadStatus(false);
            }
            catch (Exception ex)
            {
             
                Console.WriteLine(ex.ToString());
                return uploadStatus(false);
            }

        }


        public IActionResult uploadTestReturns()
        {
            try
            {
                TestReturns = new ObservableCollection<ReturnOption>() {
                new ReturnOption("Abandoned Job"), new ReturnOption("Missing Parts"), new ReturnOption("Extra Parts"),
                new ReturnOption("Repair"), new ReturnOption("Wrong Part")};

                bool updateStatus = _context.AddReturnList(TestReturns);

                return uploadStatus(updateStatus);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return uploadStatus(false);
            }

        }

        public IActionResult uploadTestTechs()
        {
            try
            {


                TestTechs = new ObservableCollection<TechOption>() { new TechOption("Tech Option 1"),
                new TechOption("Tech Option 2"), new TechOption("Tech Option 3")};

                bool updateStatus = _context.AddTechList(TestTechs);

                return uploadStatus(updateStatus);
            }
            catch (Exception ex)
            {
              
                Console.WriteLine(ex.ToString());
                return uploadStatus(false);
            }


        }

        public IActionResult uploadTestParts()
        {
            try
            {


                TestParts = new ObservableCollection<Part> { new Part("TestPart1", "TestJob1", "Tech Option 3","Missing Parts", 5),
                new Part("TestPart2", "TestJob2", "Tech Option 2","Extra Parts", 12),
                new Part("TestPart3", "TestJob3", "Tech Option 1","Repair", 1),
                new Part("TestPart4", "TestJob4", "Tech Option 2","Abandoned Job", 132),
                new Part("TestPart5", "TestJob5", "Tech Option 1","Wrong Part", 65) };

                bool updateStatus = _context.AddPartsList(TestParts);

                return uploadStatus(updateStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return uploadStatus(false);

            }


        }

        public IActionResult uploadStatus(bool updateStat)
        {
            string updateString;

            if (updateStat)
            {
                updateString = "Succesful Add";
            }
            else
            {
                updateString = "Failed to Add";
            }

          return RedirectToAction("Index", new { uploadStat = updateString });

        }

    }
}
