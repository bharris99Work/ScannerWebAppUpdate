namespace ScannerWebAppUpdate.Models
{
    public class AssignedPart
    {
        public int AssignedPartId { get; set; }
        public int PartId { get; set; }
        public int JobId { get; set; }  
        public int AssignedQuantity { get; set; }

        public string PartStatus { get; set; }

        public string ReturnReason { get; set; }
    }
}
