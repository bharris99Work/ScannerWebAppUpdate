namespace ScannerWebAppUpdate.Models
{
    public class TruckPart
    {
        public int TruckPartId { get; set; }
        public int PartId { get; set; }
        public int TruckId { get; set; }
        public int QuantityAvalible { get; set; }
        public int QuantityAllocated { get; set; }
        public int JobId {  get; set; }
    }
}
