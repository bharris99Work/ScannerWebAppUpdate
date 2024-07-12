namespace ScannerWebAppUpdate.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string ItemNumber { get; set; }
        public int Quantity { get; set; }
        public string Description {  get; set; }

        public Part(string partNumber, string desc, int quant)
        {
            ItemNumber = partNumber;
            Quantity = quant;
            Description = desc;

        }
        public Part() {
            ItemNumber = "";
            Quantity = 0;
            Description = "";
        }
    }
}
