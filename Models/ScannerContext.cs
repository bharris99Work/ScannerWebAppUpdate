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
        public DbSet<Jobs> Jobs { get; set; }

        public DbSet<JobPart> JobParts { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Tech> Techs { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<POPart> POParts { get; set; }
        public DbSet<TruckPart> TruckParts { get; set; }
        public DbSet<TechTruck> TechTrucks { get; set; }


        public string DbPath { get; }

        public ScannerContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ScannerDatabase.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

    

        //Logic for adding parts
        #region
        public async Task<bool> AddPartAsync(Part part)
        {
            try
            {
                //Looks for duplicates
                if (!Parts.Any(parts => parts.PartNumber == part.PartNumber))
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
        #endregion

        public async Task<bool> AddTech(Tech tech)
        {
            try
            {
                //Looks for duplicates
                if (!Techs.Any(techs => techs.TechName == tech.TechName))
                {
                    Techs.Add(tech);
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

        

        //Adding jobs and searching for jobs
        #region
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


        public async Task<List<PartsQuantityViewModel>> JobPartsFind(Jobs selectedJob)
        {
            try
            {
                //X = Find all AssignedParts where AssignedPart.JobId = job.JobId
                //Grab all parts where X.PartId = AssignedPart.PartId
                //Save as list 
                //Send back

                //var parts = from jobpart in JobParts
                //            join part in Parts on jobpart.PartId equals part.PartId
                //            join job in Jobs on jobpart.JobId equals job.JobsId
                //            join po 
                

                // return parts;
                throw new Exception("An error occurred while retrieving parts assigned to the job");
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                throw new Exception("An error occurred while retrieving parts assigned to the job", ex);
            }
        }
        #endregion

        //Truck Functions
        #region
        /// <summary>
        /// Functions to create and add trucks
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddTestTruck()
        {
            try
            {
                string truckName = "TestTruck";
                bool truckExist = await Trucks.AnyAsync(truck => truck.TruckName.Trim() == truckName.Trim());

                if (!truckExist) {
                    Truck testTruck = new Truck()
                    {
                        TruckName = truckName
                    };

                    Trucks.Add(testTruck);
                    await SaveChangesAsync();
                }

                else
                {
                    return false;
                }
              

                return true;
            }
            catch (Exception ex)
            {


                return false;
            }
        }
 
        public async Task<List<Part>> GetTruckParts(string truckName)
        {
            try
            {
                var parts = await (from truck in Trucks
                                   join truckPart in TruckParts on truck.TruckId equals truckPart.TruckId
                                   join part in Parts on truckPart.PartId equals part.PartId
                                   where truck.TruckName.Trim() == truckName.Trim()
                                   select part).ToListAsync();

                return parts;
            }
            catch(Exception ex)
            {

                return new List<Part>();
            }
          

        }

        public async Task<bool> AddTruck(Truck truck)
        {
            try
            {
                //Looks for duplicates
                if (!Trucks.Any(trucks => trucks.TruckName == truck.TruckName))
                {
                    Trucks.Add(truck);
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

        public async Task<bool> AssignTruck(Truck truck, Tech tech)
        {
            try
            {
                int techId = Techs.FirstOrDefault(techs => techs.TechName == tech.TechName).TechId;

                int truckId = Trucks.FirstOrDefault(trucks => trucks.TruckName == truck.TruckName).TruckId;

                TechTruck techTruck = new TechTruck()
                {
                    TechId = techId,
                    TruckId = truckId
                };

                TechTrucks.Add(techTruck);
                await SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddTruckParts(string poNumber, int truckId)
        {
            try
            {
                int poNum = PurchaseOrders.FirstOrDefault(po => po.Name == poNumber).PurchaseOrderId;

                List<POPart> poParts = POParts.Where(po => po.PurchaseOrderId == poNum).ToList();

                foreach (POPart part in poParts)
                {
                    TruckPart truckPart = new TruckPart()
                    {
                        PartId = part.PartId,
                        TruckId = truckId,
                        QuantityAvalible = part.Quantity,
                    };

                    TruckParts.Add(truckPart);
                }
                await SaveChangesAsync();   
                return true;
            }
            catch (Exception ex) {
            
            return false;
            
            }
     


        }
        #endregion

        //Purchase Order Functions
        #region
        public async Task<bool> AddPurchaseOrder(PurchaseOrder po)
        {
            try
            {
                if (!PurchaseOrders.Any(pos => pos.Name == po.Name))
                {
                    PurchaseOrders.Add(po);
                    await SaveChangesAsync();
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<bool> CreatePOParts(int numberOfParts, string POName)
        {
            try
            {
                int PONumId = PurchaseOrders.FirstOrDefault(po => po.Name == POName).PurchaseOrderId;
                if (PONumId != 0) {

                    Random rand = new Random();
                    for (int i = 0; i < numberOfParts; i++)
                    {
                        int index = rand.Next(1, Parts.ToList().Count);
                        //Random number within length of parts
                        POPart part = new POPart()
                        {
                            PurchaseOrderId = PONumId,

                            PartId = Parts.ElementAt(index).PartId,

                            Quantity = index +2,
                            Status = "Ordered",
                            ReturnStatus = ""
                        };

                        POParts.Add(part);
                    }
                }
                else
                {
                    return false;
                }
                await SaveChangesAsync ();
                return true;
            }
            catch (Exception ex) 
            {
            
                return false;
            }

        }

        public async Task<List<PartsQuantityViewModel>> GetPoParts(int poid)
        {
            //List<Part> poParts = new List<Part>();
            try
            {
                // int poid  = PurchaseOrders.FirstOrDefault(po => po.Name.Trim() == POName.Trim()).PurchaseOrderId;

                var parts = from popart in POParts
                            join part in Parts on popart.PartId equals part.PartId
                            join po in PurchaseOrders on popart.PurchaseOrderId equals po.PurchaseOrderId
                            join job in Jobs on po.JobId equals job.JobsId into jobsGroup
                            from job in jobsGroup.DefaultIfEmpty()
                            join truck in Trucks on po.TruckId equals truck.TruckId into trucksGroup
                            from truck in trucksGroup.DefaultIfEmpty()
                            where popart.PurchaseOrderId == poid
                            select new PartsQuantityViewModel
                            {
                                PurchaseOrderId = poid,
                                POName = po.Name,


                                JobId = job != null ? job.JobsId : 0,
                                JobName = job != null ? job.JobNumber : "",

                                TruckId = truck != null ? truck.TruckId : 0,
                                TruckName = truck != null ? truck.TruckName : "",

                                Status = popart.Status,
                                PartId = part.PartId,
                                PartName = part.PartNumber,
                                PartDescription = part.Description,
                                QuantityOrdered = popart.Quantity

                            };
                            

                return await parts.ToListAsync();
                                   

            }
            catch (Exception ex) {
                return null;
            
            }

        }

        public async Task<List<PurchaseOrderViewModel>> GetPOViewModels()
        {
            try
            {
                var query = from po in PurchaseOrders
                            join job in Jobs on po.JobId equals job.JobsId into jobGroup
                            from job in jobGroup.DefaultIfEmpty()
                            join truck in Trucks on po.TruckId equals truck.TruckId into truckGroup
                            from truck in truckGroup.DefaultIfEmpty()
                            select new PurchaseOrderViewModel
                            {
                                PurchaseOrderID = po.PurchaseOrderId,
                                POName = po.Name,
                                Type = po.Type,
                                JobId = job != null ? job.JobsId : 0,
                                JobName = job != null ? job.JobNumber : "",
                                TruckId = truck != null ? truck.TruckId : 0,
                                TruckName = truck != null ? truck.TruckName : "",
                            };

                return await query.ToListAsync();

            }
            catch (Exception ex) {
            
                return null ;
            
            }
         

        }


        #endregion

        /// <summary>
        /// Uploading parts list from excel, excel functions
        /// </summary>
        /// <param name="partsTable"></param>
        /// <returns></returns>
        #region
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
        #endregion

        public async Task<List<Part>> SearchPartAsync(string itemNumber, string jobNumber, string description)
        {
            var query = Parts.AsQueryable();

            if (!string.IsNullOrEmpty(itemNumber))
            {
                query = query.Where(p => p.PartNumber.ToLower().Trim().Contains(itemNumber.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(p => p.PartNumber.Trim().Contains(description));
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
