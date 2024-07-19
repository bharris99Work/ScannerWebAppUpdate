namespace ScannerWebAppUpdate.Models
{
    public class PurchaseOrderViewModel
    {
        public int PurchaseOrderID { get; set; }
        public string POName { get; set; }
        public int TruckId { get; set; }
        public string TruckName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }

        public string Type { get; set; }
    }
}
