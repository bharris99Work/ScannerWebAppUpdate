namespace ScannerWebAppUpdate.Models
{
    public class PartUpdate
    {
        public Part part { get; set; }
        public bool updated { get; set; }

        public PartUpdate(Part updatedPart, bool update)
        {
            part = updatedPart;
            updated = update;
        }

        public PartUpdate() { 
        part = null;
        updated = false;
        }
    }
}
