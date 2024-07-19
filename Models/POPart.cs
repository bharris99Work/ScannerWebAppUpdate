namespace ScannerWebAppUpdate.Models
{
    public class POPart
    {
        public int POPartId { get; set; }
        public int PartId { get; set; }

        public int PurchaseOrderId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ReturnStatus { get; set; }
    }
}
