namespace ScannerWebAppUpdate.Models
{
    public class ReturnPartsViewModel
    {
        public int ReturnPartId { get; set; }
        public string ReturnPartNumber { get; set; }
        public int JobPartId { get; set; }
        public string JobNumber { get; set; }
        public string PartNumber { get; set; }
        public int QuantityReturned { get; set; }

        public string ReturnReason {  get; set; }

        public string POName { get; set; }
        public int PurchaseOrderId { get; set; }
    }
}
