namespace ScannerWebAppUpdate.Models
{
    public class ReturnPart
    {
        public int ReturnPartId { get; set; }
        public string ReturnPartNumber { get; set; }
        public int JobPartId { get; set; }

        public int QuantityReturned { get; set; }  

        public string ReturnReason { get; set; }
    }
}
