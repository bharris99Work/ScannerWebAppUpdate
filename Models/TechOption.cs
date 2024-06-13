using System.ComponentModel.DataAnnotations;

namespace ScannerWebAppUpdate.Models
{
    public class TechOption
    {
        [Key]
        public int TechId { get; set; }

        public string Description { get; set; }

        public TechOption() { }

        public TechOption(string description) {
        Description = description;
        }   
    }
}
