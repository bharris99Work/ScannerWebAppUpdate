using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;

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

        public DbSet<ReturnPart> ReturnParts { get; set; }


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

        public async Task<bool> AddPartsList(List<Part> parts) 
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


        public async Task<bool> AddReturnPart(ReturnPart newReturnPart)
        {
            try
            {
                //Check if it exists
                if(!ReturnParts.Any(rp => rp.ReturnPartNumber == newReturnPart.ReturnPartNumber))
                {

                    var jp = JobParts.FirstOrDefault(jobpart => jobpart.JobPartId == newReturnPart.JobPartId);

                    //Find Job Part with jobpartid
                   // JobPart jp = await JobParts.FirstOrDefaultAsync(jp => jp.JobPartId == newReturnPart.JobPartId);
                   
                    //Remove available parts = to Quantity Returned
                    if (jp != null) {
                        jp.AvailableQuantity = jp.AvailableQuantity - newReturnPart.QuantityReturned;
                    };

                    ReturnParts.Add(newReturnPart);

                    await SaveChangesAsync();

                    return true;
                }
                //Save changes


                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UpdateReturnPart(ReturnPart newReturnPart, int CheckedIn)
        {
            try
            {
                //Find Job Part with jobpartid
                var jp = await JobParts.FirstOrDefaultAsync(jobpart => jobpart.JobPartId == newReturnPart.JobPartId);
                var rp = await ReturnParts.FirstOrDefaultAsync(returnpart => returnpart.ReturnPartId == newReturnPart.ReturnPartId);


                //Remove available parts = to Quantity Returned
                if (jp != null && rp != null)
                {
                    jp.AvailableQuantity = jp.AvailableQuantity + CheckedIn;

                    rp.QuantityReturned = rp.QuantityReturned - CheckedIn;

                }
                else
                {
                    return false;
                }


                await SaveChangesAsync();

                return true;
                //Save changes

            }
            catch (Exception ex)
            {
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

        public async Task<int> GetJobAsync(string jobname)
        {
            try
            {
               int jobId = Jobs.FirstOrDefault(job => job.JobNumber == jobname).JobsId;

                if (jobId != 0) {
                    return jobId;
                }
                return 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }


        public async Task<List<JobPartsViewModel>> JobPartsFind(int jobId)
        {
            try
            {
                //Using JobId

                //Find all JobParts with JobId
                //Find all Parts with JobParts.PartId equals Parts.PartId
                //Find all PurchaseOrders with JobParts.PoId = PurchaseOrders.ID

                var parts = await (from joparts in JobParts
                            join part in Parts on joparts.PartId equals part.PartId
                            join po in PurchaseOrders on joparts.PurchaseOrderId equals po.PurchaseOrderId
                            where joparts.JobId == jobId
                            select new JobPartsViewModel()
                            {
                                JobPartId = joparts.JobPartId,
                                JobId = jobId,
                                PartId = joparts.PartId,
                                PartName = part.PartNumber,
                                PartDescription = part.Description,
                                PurchaseOrderId = joparts.PurchaseOrderId,
                                POName = po.Name,
                                Status = joparts.Status,
                                AssignedParts = joparts.AssignedQuantity,
                                SignedOff = joparts.SignedOff,
                                AvailableQuantity = joparts.AvailableQuantity
                            }).ToListAsync();

                return parts;

                // return parts;
                throw new Exception("An error occurred while retrieving parts assigned to the job");
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                throw new Exception("An error occurred while retrieving parts assigned to the job", ex);
            }
        }


        public async Task<bool> AssignPart(JobPartsViewModel selectedjobPart, int quantity)
        {
            try
            {
                var foundpart = JobParts.FirstOrDefault(jobpart => jobpart.PartId == selectedjobPart.PartId && jobpart.PurchaseOrderId == selectedjobPart.PurchaseOrderId);

                if (foundpart != null) {
                    if(foundpart.AvailableQuantity - quantity >= 0)
                    {
                        int newQuantity = foundpart.AvailableQuantity - quantity;
                        int newAllocated = foundpart.AssignedQuantity + quantity;
                        foundpart.AvailableQuantity = newQuantity;
                        foundpart.AssignedQuantity = newAllocated;

                        await SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                else
                    return false;  
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddTruckPart(JobPartsViewModel selectedPart)
        {
            try
            {
                var foundTruckPart = TruckParts.FirstOrDefault(truckpart => selectedPart.TruckId == truckpart.TruckId
                && truckpart.PartId == selectedPart.PartId && truckpart.PurchaseOrderId == selectedPart.PurchaseOrderId);

                var existingJobPart = JobParts.FirstOrDefault(jobpart => selectedPart.PartId
                == jobpart.PartId && selectedPart.PurchaseOrderId == jobpart.PurchaseOrderId && selectedPart.JobId == jobpart.JobId);

                //Checks if truck part exists
                if (foundTruckPart != null)
                {

                    //Update Existing Part if available
                    if (existingJobPart != null)
                    {
                        existingJobPart.AvailableQuantity += selectedPart.AssignedParts;

                        foundTruckPart.QuantityAvalible -= selectedPart.AssignedParts;
                        foundTruckPart.QuantityAllocated += selectedPart.AssignedParts;

                        await SaveChangesAsync();
                        return true;
                    }

                    else
                    {
                        JobPart newPart = new JobPart()
                        {
                            PartId = selectedPart.PartId,
                            truckId = selectedPart.PartId,
                            PurchaseOrderId = selectedPart.PurchaseOrderId,
                            AssignedQuantity = 0,
                            AvailableQuantity = selectedPart.AssignedParts,
                            Status = "Added From Truck",
                            JobId = selectedPart.JobId
                        };

                        foundTruckPart.QuantityAvalible -= selectedPart.AssignedParts;
                        foundTruckPart.QuantityAllocated += selectedPart.AssignedParts;

                        JobParts.Add(newPart);
                        await SaveChangesAsync();
                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            
            }
        }

        public async Task<bool> UpdateJobPart(JobPartsViewModel selectedjobPart)
        {
            try
            {
                var foundpart = JobParts.FirstOrDefault(jobpart => jobpart.PartId == selectedjobPart.PartId && jobpart.PurchaseOrderId == selectedjobPart.PurchaseOrderId);

                if (foundpart != null)
                {
                    foundpart.AssignedQuantity = selectedjobPart.AssignedParts;
                    foundpart.AvailableQuantity = selectedjobPart.AvailableQuantity;
                    if(selectedjobPart.Status != null && selectedjobPart.Status != string.Empty)
                    {
                        foundpart.Status = "Parts Used";
                    }

                    await SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SignOffParts(JobPartsViewModel selectedjobPart)
        {
            try
            {
                var foundpart = JobParts.FirstOrDefault(jobpart => jobpart.PartId == selectedjobPart.PartId && jobpart.PurchaseOrderId == selectedjobPart.PurchaseOrderId);

                if (foundpart != null)
                {
                    foundpart.SignedOff = selectedjobPart.SignedOff;
                    
                    if (selectedjobPart.Status != null && selectedjobPart.Status != string.Empty)
                    {
                        foundpart.Status = "Signed Off Parts";
                    }

                    await SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public async Task<bool> RemovePart(JobPartsViewModel selectedjobPart, int quantity)
        {
            try
            {
                var foundpart = JobParts.FirstOrDefault(jobpart => jobpart.PartId == selectedjobPart.PartId && jobpart.PurchaseOrderId == selectedjobPart.PurchaseOrderId);

                if (foundpart != null)
                {
                    if (foundpart.AssignedQuantity - quantity >= 0)
                    {
                        int newAllocated = foundpart.AssignedQuantity - quantity;
                        int newQuantity = foundpart.AvailableQuantity + quantity;
                        foundpart.AssignedQuantity = newAllocated;
                        foundpart.AvailableQuantity = newQuantity;
                        

                        await SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<JobPartsPartialViewModel> GetJobPartPartialVM (int jobId)
        {
            try
            {
                List<JobPartsViewModel> jobParts = await JobPartsFind(jobId);
                List<JobPartsViewModel> assignedParts = jobParts.FindAll(jp => jp.AssignedParts > 0);


                //Get List of available truck parts
                JobPartsPartialViewModel jppv = new JobPartsPartialViewModel()
                {
                    jobId = jobId,
                    AssignedParts = assignedParts,
                    AvailableParts = jobParts,
                    AvailableTruckParts = await GetTechParts(1)
                };
                return jppv;

            }
            catch (Exception ex)
            {
                return new JobPartsPartialViewModel();
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

        public async Task<bool> AddTruckParts(int poNum, int truckId)
        {
            try
            {
                //int poNum = PurchaseOrders.FirstOrDefault(po => po.Name == poNumber).PurchaseOrderId;

                List<POPart> poParts = POParts.Where(po => po.PurchaseOrderId == poNum).ToList();

                foreach (POPart part in poParts)
                {
                    TruckPart truckPart = new TruckPart()
                    {
                        PartId = part.PartId,
                        TruckId = truckId,
                        QuantityAvalible = part.Quantity,
                        PurchaseOrderId = part.PurchaseOrderId
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

        public async Task<List<JobPartsViewModel>> GetTechParts(int TechId)
        {
            var parts = await (from tech in Techs
                               join techtruck in TechTrucks on tech.TechId equals techtruck.TechId
                               join truckpart in TruckParts on techtruck.TruckId equals truckpart.TruckId
                               join part in Parts on truckpart.PartId equals part.PartId
                               join po in PurchaseOrders on truckpart.PurchaseOrderId equals po.PurchaseOrderId
                               where tech.TechId == TechId
                               select new JobPartsViewModel()
                               {
                                   //JobPartId = truckpart.JobPartId,
                                   //JobId = jobId,
                                   PartId = part.PartId,
                                   PartName = part.PartNumber,
                                   PartDescription = part.Description,
                                   PurchaseOrderId = po.PurchaseOrderId,
                                   POName = po.Name,
                                   TruckId = truckpart.TruckId,
                                   //Status = joparts.Status,
                                   AssignedParts = 0,
                                   AvailableQuantity = truckpart.QuantityAvalible
                               }).ToListAsync();
            return parts;
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

        public async Task<int> CreatePOParts(int numberOfParts, string POName)
        {
            try
            {
                List<POPart> tempPoPartList = new List<POPart>();

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
                        //Check if part exist in popart with poid and part id
                        if (!tempPoPartList.Any(popart => popart.PartId == part.PartId))
                        {
                            POParts.Add(part);
                            tempPoPartList.Add(part);
                        };

                        if(tempPoPartList.Count < numberOfParts)
                        {
                            i -= 1;
                        }
                    }
                }
                else
                {
                    return 0;
                }
                await SaveChangesAsync ();
                return PONumId;
            }
            catch (Exception ex) 
            {
            
                return 0;
            }

        }

        //TEST DEMO
        public async Task<int> CreatePOParts(List<Part> parts, string POName)
        {
            try
            {
                int PONumId = PurchaseOrders.FirstOrDefault(po => po.Name == POName).PurchaseOrderId;
                if (PONumId != 0)
                {
                    Random rand = new Random();
                    //Create POPart
                    //Part ID and Quantity
                    foreach (var part in parts) {
                        int partid = Parts.FirstOrDefault(dbpart => dbpart.PartNumber == part.PartNumber).PartId;


                        POPart pOPart = new POPart()
                        {
                            PartId = partid,
                            PurchaseOrderId = PONumId,
                            Status = "Ordered",
                            ReturnStatus = "N/A",
                            Quantity = rand.Next(1, 100),
                        };

                        POParts.Add(pOPart);
                    };
                }
                else
                {
                    return 0;
                }
                await SaveChangesAsync();
                return PONumId;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<bool> CreateJobOrder(int jobid, int POId)
        {
            try
            {
                //Using PO grab all PoParts
                //Create new JobParts
                var parts = await (from popart in POParts
                            where popart.PurchaseOrderId == POId
                            select new JobPart()
                            {
                                PartId = popart.PartId,
                                JobId = jobid,
                                PurchaseOrderId = popart.PurchaseOrderId,
                                AvailableQuantity = popart.Quantity,
                                Status = "Ordered"
                               
                            }).ToListAsync();
               
                JobParts.AddRange(parts);




                await SaveChangesAsync();
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


        //ReturnPart Functions
        #region

        public async Task< List<ReturnPartsViewModel>> GetReturnPartsVM()
        {
            try
            {
                var parts = await (from returnpart in ReturnParts
                                   join jobpart in JobParts on returnpart.JobPartId equals jobpart.JobPartId
                                   join part in Parts on jobpart.PartId equals part.PartId
                                   join job in Jobs on jobpart.JobId equals job.JobsId
                                   join po in PurchaseOrders on jobpart.PurchaseOrderId equals po.PurchaseOrderId
                                   select new ReturnPartsViewModel()
                                   {
                                       ReturnPartId = returnpart.ReturnPartId,
                                       ReturnPartNumber = returnpart.ReturnPartNumber,
                                       JobPartId = jobpart.JobPartId,
                                       JobNumber = job.JobNumber,
                                       PartNumber = part.PartNumber,
                                       QuantityReturned = returnpart.QuantityReturned,
                                       ReturnReason = returnpart.ReturnReason,
                                       POName = po.Name,
                                       PurchaseOrderId = po.PurchaseOrderId
                                   }).ToListAsync();
                return parts;
            }
            catch (Exception ex)
            {
                return null;
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
