namespace ScannerWebAppUpdate.Models
{
    public class JobPartsViewModel
    {
        public int JobPartId { get; set; }
        public int JobId {  get; set; }
        public int PurchaseOrderId { get; set; }
        public string POName { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
        public string PartDescription { get; set; }
        public string Status { get; set; }
        public int TruckId { get; set; }
        public string TruckName { get; set; }
        public int AssignedParts {  get; set; }
        public int AvailableQuantity { get; set; }
    }
}
