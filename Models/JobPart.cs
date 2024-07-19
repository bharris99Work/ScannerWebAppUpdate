namespace ScannerWebAppUpdate.Models
{
    public class JobPart
    {
        public int JobPartId { get; set; }

        public int PartId { get; set; }

        public int JobId { get; set; }

        public string Status { get; set; }

        public int UsedQuantity { get; set; }

        public string PartType { get; set; }
    }
}
