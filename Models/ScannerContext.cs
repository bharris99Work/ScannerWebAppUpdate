using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
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

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        public void AddPart(Part part)
        {
            try
            {
                //Looks for duplicates
                if (!Parts.Any(parts => parts.ItemNumber == part.ItemNumber))
                {
                    Parts.Add(part);
                    SaveChanges();
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            
            }

        }

        public void AddPartsList(ObservableCollection<Part> parts) 
        {
            try
            {
                foreach (Part part in parts)
                {
                    AddPart(part);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }



        public void AddTechList(ObservableCollection<TechOption> TestTechOptions)
        {
            try
            {
                foreach (TechOption tech in TestTechOptions)
                {
                    AddTech(tech);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void AddReturnList(ObservableCollection<ReturnOption> TestReturnOptions)
        {
            try
            {
                foreach (ReturnOption returnOption in TestReturnOptions)
                {
                    AddReturn(returnOption);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void AddTech(TechOption techOption)
        {
            try
            {
                //Looks for duplicates
                if (!TechOptions.Any(tech => tech.Description == techOption.Description))
                {
                    TechOptions.Add(techOption);
                    SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

        }

        public void AddReturn(ReturnOption returnOption)
        {
            try
            {
                //Looks for duplicates
                if (!ReturnOptions.Any(returnOpt => returnOpt.Description == returnOption.Description))
                {
                    ReturnOptions.Add(returnOption);
                    SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }


        }
    }
}
