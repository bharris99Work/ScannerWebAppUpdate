namespace ScannerWebAppUpdate.Models
{
    public class JobHistory
    {
        public int JobHistoryId { get; set; }
        public int TechId { get; set; }
        public int JobPartId { get; set; }
        public string Action { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
