namespace ScannerWebAppUpdate.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string ItemNumber { get; set; }
        public string JobNumber { get; set; }

       
        public string ReturnOption { get; set; }

        public string TechOption { get; set; }

        public int Quantity { get; set; }

        public Part(string partNumber, string jobNumber)
        {
            ItemNumber = partNumber;
            JobNumber = jobNumber;
 
            ReturnOption = "N/A";
            TechOption = "N/A";

            Quantity = 0;
        }
        public Part() { }
    }
}
