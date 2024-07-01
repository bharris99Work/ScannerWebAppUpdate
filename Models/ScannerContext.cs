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

        public bool AddPart(Part part)
        {
            try
            {
                //Looks for duplicates
                if (!Parts.Any(parts => parts.ItemNumber == part.ItemNumber))
                {
                    Parts.Add(part);
                    SaveChanges();
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

        public bool AddPartsList(ObservableCollection<Part> parts) 
        {
            try
            {
                foreach (Part part in parts)
                {
                    if (!AddPart(part))
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



        public bool AddTechList(ObservableCollection<TechOption> TestTechOptions)
        {
            try
            {
                foreach (TechOption tech in TestTechOptions)
                {
                    if (!AddTech(tech)){
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

        public bool AddReturnList(ObservableCollection<ReturnOption> TestReturnOptions)
        {
            try
            {
                foreach (ReturnOption returnOption in TestReturnOptions)
                {
                    if (!AddReturn(returnOption)){
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

        public bool AddTech(TechOption techOption)
        {
            try
            {
                //Looks for duplicates
                if (!TechOptions.Any(tech => tech.Description == techOption.Description))
                {
                    TechOptions.Add(techOption);
                    SaveChanges();
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

        public bool AddReturn(ReturnOption returnOption)
        {
            try
            {
                //Looks for duplicates
                if (!ReturnOptions.Any(returnOpt => returnOpt.Description == returnOption.Description))
                {
                    ReturnOptions.Add(returnOption);
                    SaveChanges();
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

        public bool UpdatePart(Part newPart)
        {
            try
            {
                var part = Parts.FirstOrDefault(p => p.PartId == newPart.PartId);

                if (part != null)
                {
                    Part foundPart = part;

                    foundPart.ItemNumber = newPart.ItemNumber;
                    foundPart.JobNumber = newPart.JobNumber;
                    foundPart.Quantity = newPart.Quantity;
                    foundPart.TechOption = newPart.TechOption;
                    foundPart.ReturnOption = newPart.ReturnOption;
                    SaveChanges();
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


        public bool UploadPartsFromExcel(DataTable partsTable)
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
                    AddPart(part);
                }
                SaveChanges();
                return true;

            }
            catch (Exception ex) {
                

                return false;
            }
        }
    }
}
