namespace ScannerWebAppUpdate.Models
{
    public class AssignedPart
    {
        public int AssignedPartId { get; set; }
        public int PartId { get; set; }
        public int JobId { get; set; }  
        public int Quantity { get; set; }
    }
}
