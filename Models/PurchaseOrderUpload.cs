namespace ScannerWebAppUpdate.Models
{
    public class PurchaseOrderUpload
    {
        public string POName { get; set; }
        public string SelectedJob { get; set; }
        public string SelectedTruck { get; set; }
        public List<OrderedParts> parts { get; set; }
    }
}
