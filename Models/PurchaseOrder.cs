namespace ScannerWebAppUpdate.Models
{
    public class PurchaseOrder
    {
     public int PurchaseOrderId { get; set; }
        public string Name { get; set; }
        public int JobId { get; set; }
        public int TruckId { get; set; }
        public string Type { get; set; }    

    }
}
