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

        public async Task<IActionResult> UploadTech(string techDescription)
        {
            try
            {
                if (techDescription != null & techDescription != string.Empty)
                {
                    TechOption techOption = new TechOption(techDescription);
                    bool updateStatus = await _context.AddTech(techOption);
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
                testPart.ReturnOption = "";
                testPart.TechOption = "";
                uploadstatus = await _context.AddPartAsync(testPart);
            }
            return uploadStatus(uploadstatus);
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

        public async Task<IActionResult> uploadTestTechs()
        {
            try
            {


                TestTechs = new ObservableCollection<TechOption>() { new TechOption("Tech Option 1"),
                new TechOption("Tech Option 2"), new TechOption("Tech Option 3")};

                bool updateStatus = await _context.AddTechList(TestTechs);

                return uploadStatus(updateStatus);
            }
            catch (Exception ex)
            {
              
                Console.WriteLine(ex.ToString());
                return uploadStatus(false);
            }


        }

        public async Task<IActionResult> uploadTestParts()
        {
            try
            {
                TestParts = new ObservableCollection<Part> { new Part("TestPart1", "TestJob1", "Tech Option 3","Missing Parts", 5),
                new Part("TestPart2", "TestJob2", "Tech Option 2","Extra Parts", 12),
                new Part("TestPart3", "TestJob3", "Tech Option 1","Repair", 1),
                new Part("TestPart4", "TestJob4", "Tech Option 2","Abandoned Job", 132),
                new Part("TestPart5", "TestJob5", "Tech Option 1","Wrong Part", 65) };

                bool updateStatus = await _context.AddPartsList(TestParts);

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
            return RedirectToAction("Index")     ;
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
