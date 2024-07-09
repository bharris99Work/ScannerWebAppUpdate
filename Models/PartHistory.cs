namespace ScannerWebAppUpdate.Models
{
    public class PartHistory
    {
        public int PartHistoryId { get; set; }

        public int PartId { get; set; }

        public DateTime DateChanged { get; set; }

        public string ChangedValues { get; set; }


        public PartHistory() { 
        
        }
    }
}
