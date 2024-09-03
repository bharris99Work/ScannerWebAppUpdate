using Microsoft.AspNetCore.Mvc;
using ScannerWebAppUpdate.Models;
using Syncfusion.XlsIO;
using System.Collections.ObjectModel;
using System.Data;

namespace ScannerWebAppUpdate.Controllers
{
    public class SettingsController : Controller
    {


        private readonly ScannerContext _context = new ScannerContext();
        private ObservableCollection<ReturnOption>? TestReturns;
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

        public async Task<IActionResult> UploadTestTruck()
        {
            await _context.AddTestTruck();

            return uploadStatus(true);
        }

        public async Task<IActionResult> uploadTestTruckParts()
        {
            //await _context.AddPartsToTestTruck();

            return uploadStatus(true);
        }


        public async Task<IActionResult> UploadReturn(string returnDescription)
        {
            try
            {
                if (returnDescription != null & returnDescription != string.Empty)
                {
                    ReturnOption returnOption = new ReturnOption(returnDescription);
                    bool updateStatus = await _context.AddReturn(returnOption);
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

        [HttpPost]
        public async Task<IActionResult> uploadPart(Part testPart)
        {
            bool uploadstatus = false;
            if (testPart != null)
            {
                uploadstatus = await _context.AddPartAsync(testPart);
            }
            return uploadStatus(uploadstatus);
        }



        [HttpPost]
        public async Task<IActionResult> UploadJob(string jobNumber, string location)
        {
            if(jobNumber != null && jobNumber != string.Empty && location != null && location != string.Empty)
            {
                Jobs job = new Jobs(jobNumber, location);

                bool upload= await _context.AddJobAsync(job);

                return uploadStatus(upload);
            }

            return uploadStatus(false);
        }


        //One Click Demo Button
        //[HttpPost]
        public async Task<IActionResult> UploadDemo()
        {
            try
            {
                //Add Tech
                Tech newTech = new Tech()
                {
                    TechId = 1,
                    TechName = "Demo Tech"
                };

                await _context.AddTech(newTech);

                //Add Tech Truck
                await _context.AddTechTruck(1, 1);

                bool success = false;
                //Create List Of Parts
                List<Part> jobPartList = new List<Part>() {
            new Part("Part001", "Test Part 1, for testing."),
             new Part("Part002", "Test Part 2, for testing."),
              new Part("Part003", "Test Part 3, for testing."),
               new Part("Part004", "Test Part 4, for testing."),

            };

                List<Part> truckPartList = new List<Part>()
            {
                 new Part("TruckPart001", "Test TruckPart 1, for testing."),
                 new Part("TruckPart002", "Test TruckPart 2, for testing.")
            };

                //Upload to DB
                success = await _context.AddPartsList(jobPartList);
                success = await _context.AddPartsList(truckPartList);

                //Get Parts as Ordered Parts


                //Create Job
                Jobs testJob = new Jobs("PM0001", "Lumberton, NC");

                //Upload to DB
                success = await _context.AddJobAsync(testJob);

                //Upload and Create PO's
                PurchaseOrder pojob = new PurchaseOrder()
                {
                    Name = "PO-0001",
                    Type = "Job Order"
                };

                PurchaseOrder potruck = new PurchaseOrder()
                {
                    Name = "PO-0002",
                    Type = "Truck Order"
                };

                int poJobId = await _context.AddPurchaseOrder(pojob);
                int poTruckId = await _context.AddPurchaseOrder(potruck);

                //Create POParts (Parts List, PO Name)
                //Send Parts list
                //Find the ID for each one
                int jobPOId = await _context.CreateTestPOParts(jobPartList, poJobId);
                int truckPOId = await _context.CreateTestPOParts(truckPartList, poTruckId);


                //Grab JobId
                int jobid = await _context.GetJobAsync(testJob.JobNumber);



                //Create JobOrder
                success = await _context.CreateJobOrder(jobid, jobPOId);

                //Create StockOrder
                success = await _context.AddTruckParts(truckPOId, 1);


                return uploadStatus(success);
            }
            catch (Exception ex)
            {
                return uploadStatus(false);
            }

        }


        public async Task<IActionResult> uploadTestReturns()
        {
            try
            {
                TestReturns = new ObservableCollection<ReturnOption>() {
                new ReturnOption("Abandoned Job"), new ReturnOption("Missing Parts"), new ReturnOption("Extra Parts"),
                new ReturnOption("Repair"), new ReturnOption("Wrong Part")};

                bool updateStatus = await _context.AddReturnList(TestReturns);

                return uploadStatus(updateStatus);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return uploadStatus(false);
            }

        }


        public async Task<IActionResult> uploadTestParts(string ItemNumber, string Description, int Quantity)
        {
            try
            {
                Part newPart = new Part() {
                PartNumber = ItemNumber,
                Description = Description,
              
                };
               


                return uploadStatus(true);
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

        public async Task<IActionResult> SeedTestData()
        {
            try
            {
                bool success = false;

                //Create Test Parts and Jobs
                for (int i = 1; i < 10; i++)
                {
                    Part tempPart = new Part()
                    {
                        PartNumber = "TestPart" + i,
                        Description = "Test Description for test part " + i
                    };

                    Jobs tempJob = new Jobs()
                    {
                        JobNumber = "TestJob" + i,
                        DateCreated = DateTime.Now,
                        Location = "Lumberton",
                        Status = "In Development"

                    };

                    success = await _context.AddPartAsync(tempPart);
                    success = await _context.AddJobAsync(tempJob);
                }


                //Create Test Trucks and techs
                Truck tempTruck = new Truck()
                {
                    TruckName = "TestTruck"
                };

                Tech tempTech = new Tech()
                {
                    TechName = "Admin"
                };

                success = await _context.AddTruck(tempTruck);
                success = await _context.AddTech(tempTech);

               //Assign Truck to tech
               success = await _context.AssignTruck(tempTruck, tempTech);


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {


                return RedirectToAction("Index");
            }
        }



        /// <summary>
        /// Takes in parts excel file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadPartsFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("File is null");
            }
            else
            {
                //Take excel file
                //Send to service
                //Service:
                    //Takes and matches header with specified attribute.
                        //Part Number = partNumber, etc....
                        //creates parts list 
                        //sends parts list to db
                        //Show finished update


                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                DataTable dataTable = ReadExcelFile(file);

                if(dataTable != null && dataTable.Rows.Count > 1)
                {
                   bool uploadStatus = await _context.UploadPartsFromExcelAsync(dataTable);
                }

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return RedirectToAction("Index");
        }


        private DataTable ReadExcelFile(IFormFile file)
        {
            DataTable dataTable = new DataTable();

            try
            {


                using (var stream = file.OpenReadStream())
                {
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = application.Workbooks.Open(stream);
                        IWorksheet worksheet = workbook.Worksheets[0]; // Assuming data is in the first worksheet

                        // Add columns based on the first row
                        var headerRow = worksheet.Rows[0];
                        foreach (var headerCell in headerRow.Cells)
                        {
                            dataTable.Columns.Add(headerCell.DisplayText);
                        }

                        // Add rows
                        for (int row = 1; row <= worksheet.Rows.Length-1; row++)
                        {
                            var excelRow = worksheet.Rows[row];
                            DataRow dataRow = dataTable.NewRow();
                            for (int col = 0; col < worksheet.Rows[row].Cells.Length; col++)
                            {
                                dataRow[col] = worksheet.Rows[row].Cells[col].DisplayText;
                            }
                            dataTable.Rows.Add(dataRow);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return dataTable;
        }

    }
}
