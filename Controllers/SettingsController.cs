using Microsoft.AspNetCore.Mvc;
using ScannerWebAppUpdate.Models;
using System.Collections.ObjectModel;

namespace ScannerWebAppUpdate.Controllers
{
    public class SettingsController : Controller
    {


        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<ReturnOption>? TestReturns;
        private ObservableCollection<TechOption>? TestTechs;
        private ObservableCollection<Part>? TestParts;


        public SettingsController() { 
            _context.Database.EnsureCreated();
        }
        public IActionResult Index()
        {
            return View();
        }


        public void uploadTestReturns()
        {
            TestReturns = new ObservableCollection<ReturnOption>() {  
                new ReturnOption("Abandoned Job"), new ReturnOption("Missing Parts"), new ReturnOption("Extra Parts"),
                new ReturnOption("Repair"), new ReturnOption("Wrong Part")
            };

            _context.AddReturnList(TestReturns);

        }

        public void uploadTestTechs()
        {
            TestTechs = new ObservableCollection<TechOption>() { new TechOption("Tech Option 1"),
                new TechOption("Tech Option 2"), new TechOption("Tech Option 3")};

            _context.AddTechList(TestTechs);

        }

        public void uploadTestParts()
        {

            TestParts = new ObservableCollection<Part> { new Part("TestPart1", "TestJob1", "Tech Option 3","Missing Parts", 5),
                new Part("TestPart2", "TestJob2", "Tech Option 2","Extra Parts", 12),
                new Part("TestPart3", "TestJob3", "Tech Option 1","Repair", 1),
                new Part("TestPart4", "TestJob4", "Tech Option 2","Abandoned Job", 132),
                new Part("TestPart5", "TestJob5", "Tech Option 1","Wrong Part", 65) };

            _context.AddPartsList(TestParts);
        }

    }
}
