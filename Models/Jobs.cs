using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
namespace ScannerWebAppUpdate.Models
{
    public class Jobs
    {
        public int JobsId { get; set; }
        public string JobNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public Jobs()
        {
            JobNumber = string.Empty;
            DateCreated = DateTime.Now;
            Location = string.Empty;
            Status = string.Empty;
        }


        public Jobs(string jobnum, string loc)
        {
            JobNumber = jobnum;
            Location = loc;
            Status = "";
            DateCreated = DateTime.Now;

        }
    }
}
