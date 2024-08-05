namespace ScannerWebAppUpdate.Models
{
    public class JobPartsPartialViewModel
    {
        public int jobId{ get; set; }
        public List<JobPartsViewModel> AssignedParts = new List<JobPartsViewModel>();
        public List<JobPartsViewModel> AvailableParts = new List<JobPartsViewModel>();
        public List<JobPartsViewModel> AvailableTruckParts = new List<JobPartsViewModel>();

    }
}
