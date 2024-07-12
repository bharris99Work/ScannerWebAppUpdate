using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq.Expressions;

namespace ScannerWebAppUpdate.Models
{
    public class ScannerContext : DbContext
    {
        public DbSet<Part> Parts { get; set; }
        public DbSet<ReturnOption> ReturnOptions { get; set; }
        public DbSet<TechOption> TechOptions { get; set; }
        public DbSet<PartHistory> PartHistory { get; set; }
        public DbSet<AssignedPart> AssignedParts { get; set; }
        public DbSet<Jobs> Jobs { get; set; }


        public string DbPath { get; }

        public ScannerContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ScannerDatabase.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        public async Task<bool> AddJobAsync(Jobs job)
        {
            try
            {
                if (!Jobs.Any(jobs => jobs.JobNumber == job.JobNumber))
                {
                    Jobs.Add(job);
                    await SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> AddPartAsync(Part part)
        {
            try
            {
                //Looks for duplicates
                if (!Parts.Any(parts => parts.ItemNumber == part.ItemNumber))
                {
                    Parts.Add(part);
                    await SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<bool> AddPartsList(ObservableCollection<Part> parts) 
        {
            try
            {
                foreach (Part part in parts)
                {
                    if (!await AddPartAsync(part))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }



        public async Task<bool> AddTechList(ObservableCollection<TechOption> TestTechOptions)
        {
            try
            {
                foreach (TechOption tech in TestTechOptions)
                {
                    if (!await AddTech(tech)){
                        return false;
                    }
                      
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<bool> AddReturnList(ObservableCollection<ReturnOption> TestReturnOptions)
        {
            try
            {
                foreach (ReturnOption returnOption in TestReturnOptions)
                {
                    if (!await AddReturn(returnOption)){
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<bool> AddTech(TechOption techOption)
        {
            try
            {
                //Looks for duplicates
                if (!TechOptions.Any(tech => tech.Description == techOption.Description))
                {
                    TechOptions.Add(techOption);
                    await SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }

        }

        public async Task<bool> AddReturn(ReturnOption returnOption)
        {
            try
            {
                //Looks for duplicates
                if (!ReturnOptions.Any(returnOpt => returnOpt.Description == returnOption.Description))
                {
                    ReturnOptions.Add(returnOption);
                    await SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;

            }
        }

        public async Task<bool> UpdatePart(Part newPart)
        {
            try
            {
                var part = Parts.FirstOrDefault(p => p.PartId == newPart.PartId);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> AddToPartHistory(Part newPart, Part oldPart)
        {
            try
            {
                //string changes = "";

                //if (newPart.ItemNumber != oldPart.ItemNumber)
                //{
                //    changes += "OldItemNumber: " + oldPart.ItemNumber + " to NewItemNumber: " + newPart.ItemNumber + "; ";
                //}
                //if (newPart.JobNumber != oldPart.JobNumber)
                //{
                //    changes += "OldJobNumber: " + oldPart.JobNumber + " to NewJobNumber: " + newPart.JobNumber + "; ";
                //}
                //if (newPart.Quantity != oldPart.Quantity)
                //{
                //    changes += "OldQuantity: " + oldPart.Quantity + " to NewQuantity: " + newPart.Quantity + "; ";
                //}
                //if (newPart.ReturnOption != oldPart.ReturnOption)
                //{
                //    changes += "OldReturn: " + oldPart.ReturnOption + " to NewReturn: " + newPart.ReturnOption + "; ";
                //}
                //if (newPart.TechOption != oldPart.TechOption)
                //{
                //    changes += "OldTech: " + oldPart.TechOption + " to+ NewTech: " + newPart.TechOption + "; ";
                //}

                //PartHistory partHistory = new PartHistory()
                //{
                //    PartId = newPart.PartId,
                //    DateChanged = DateTime.Now,
                //    ChangedValues = changes
                //};

                //PartHistory.Add(partHistory);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
                return false;
            }
            

        }

        public async Task<List<Part>> JobPartsFind(int JobsId)
        {
            try
            {
                //X = Find all AssignedParts where AssignedPart.JobId = job.JobId
                //Grab all parts where X.PartId = AssignedPart.PartId
                //Save as list 
                //Send back

                var parts = await (from ap in AssignedParts
                                   join p in Parts on ap.PartId equals p.PartId
                                   where ap.JobId == JobsId
                                   select p).Distinct().ToListAsync();

                return parts;
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                throw new Exception("An error occurred while retrieving parts assigned to the job", ex);
            }
        }

        public async Task<bool> UploadPartsFromExcelAsync(DataTable partsTable)
        {
            try
            {
                foreach(DataRow row in partsTable.Rows)
                {
                    var part = new Part
                    {
                        //ItemNumber = row["PartNumber"].ToString(),
                        //JobNumber = row["JobName"].ToString(),
                        //Quantity = int.Parse(row["Quantity"].ToString()),
                        //ReturnOption = row["ReturnReason"].ToString()
                    };
                    await AddPartAsync(part);
                }
                await SaveChangesAsync();
                return true;

            }
            catch (Exception ex) {
                

                return false;
            }
        }

        public async Task<List<Part>> SearchPartAsync(string itemNumber, string jobNumber, string description)
        {
            var query = Parts.AsQueryable();

            if (!string.IsNullOrEmpty(itemNumber))
            {
                query = query.Where(p => p.ItemNumber.ToLower().Trim().Contains(itemNumber.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(p => p.ItemNumber.Trim().Contains(description));
            }
            if (!string.IsNullOrEmpty(jobNumber))
            {
                //query = query.Where(p => p.JobNumber.ToLower().Trim().Contains(jobNumber.ToLower().Trim()));
            }

            var items = await query.ToListAsync();

            return items;
        }
    }
}
