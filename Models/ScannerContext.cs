using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace ScannerWebAppUpdate.Models
{
    public class ScannerContext : DbContext
    {
        public DbSet<Part> Parts { get; set; }
        public DbSet<ReturnOption> ReturnOptions { get; set; }
        public DbSet<TechOption> TechOptions { get; set; }


        public string DbPath { get; }

        public ScannerContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ScannerDatabase.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

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

                if (part != null)
                {
                    Part foundPart = part;

                    foundPart.ItemNumber = newPart.ItemNumber;
                    if(foundPart.JobNumber != null && foundPart.JobNumber != string.Empty)
                    {
                        foundPart.JobNumber = newPart.JobNumber;

                    }
                    else
                        foundPart.JobNumber = "";

                    foundPart.Quantity = newPart.Quantity;
                    foundPart.TechOption = newPart.TechOption;
                    foundPart.ReturnOption = newPart.ReturnOption;
                    await SaveChangesAsync();
                    return true;
                }
                else
                {
                    Console.Write("Part not found");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
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
                        ItemNumber = row["PartNumber"].ToString(),
                        JobNumber = row["JobName"].ToString(),
                        Quantity = int.Parse(row["Quantity"].ToString()),
                        ReturnOption = row["ReturnReason"].ToString()
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
                query = query.Where(p => p.JobNumber.ToLower().Trim().Contains(jobNumber.ToLower().Trim()));
            }

            var items = await query.ToListAsync();

            return items;
        }
    }
}
