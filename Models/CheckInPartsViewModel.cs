namespace ScannerWebAppUpdate.Models
{
    public class CheckInPartsViewModel
    {
        public List<JobPartsViewModel> checkParts {  get; set; }
        public int notChecked { get; set; }

        public int totalParts { get; set; }
    }
}
