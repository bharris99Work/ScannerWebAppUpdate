namespace ScannerWebAppUpdate.Models
{
    public class PartDelivery
    {
        public int PartDeliveryId { get; set; }

        public int PartId { get; set; }


        public DateTime OrderedDate { get; set; }

        public DateTime EstimatedArrival { get; set; }

        public string CurrentStatus { get; set; }

        public PartDelivery()
        {

        }


    }
}
