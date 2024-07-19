namespace ScannerWebAppUpdate.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string PartNumber { get; set; }
        public string Description {  get; set; }

        public Part(string partNumber, string desc)
        {
            PartNumber = partNumber;
            Description = desc;

        }
        public Part() {
            PartNumber = "";
            Description = "";
        }
    }
}
