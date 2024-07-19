namespace ScannerWebAppUpdate.Models
{
    public class PartsQuantityViewModel
    {
        public int PartId { get; set; }
        public string PartName { get; set; }
        public string PartDescription { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int TruckId { get; set; }
        public string TruckName { get; set; }   
        public int PurchaseOrderId { get; set; }
        public string POName { get; set; }
        public string Status { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityAssigned { get; set; }
        public int QuantityUnassigned { get; set; }
    }
}
