using System.ComponentModel.DataAnnotations;

namespace ScannerWebAppUpdate.Models
{
    public class ReturnOption
    {
        [Key]
        public int ReturnId { get; set; }
        public string Description { get; set; }

        public ReturnOption() { }

        public ReturnOption(string description)
        {

            Description = description;
        }
    }
}
