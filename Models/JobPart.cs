namespace ScannerWebAppUpdate.Models
{
    public class JobPart
    {
        public int JobPartId { get; set; }

        public int PartId { get; set; }

        public int JobId { get; set; }

        public int truckId {  get; set; }
        public int PurchaseOrderId { get; set; }

        public string Status { get; set; }

        public int AssignedQuantity { get; set; }
        public int AvailableQuantity { get; set; }

    }
}
