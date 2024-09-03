namespace ScannerWebAppUpdate.Models
{
    public class JobHistoryViewModel
    {
        public string PartNumber { get; set; }
        public string TechName { get; set; }
        public string JobNumber { get; set; }
        public string Action { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
