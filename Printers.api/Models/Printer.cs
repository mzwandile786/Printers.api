using Printers.api;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printers.api.Models
{
    [Table("Printers")]
    public class Printer
    {
        [Key]
        public int EngenPrintersID { get; set; }

        [Required]
        [StringLength(30)]
        public string PrinterName { get; set; }

        [Required]
        [StringLength(50)]
        public string FolderToMonitor { get; set; }

        [Required]
        [StringLength(50)]
        public string OutputType { get; set; }

        [Required]
        [StringLength(50)]
        public string FileOutput { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int PrinterMakeID { get; set; }

        [Required]
        public DateTime CreatedTimeStamp { get; set; }

        [ForeignKey("PrinterMakeID")]
        public PrinterMake? PrinterMake { get; set; }  // marked nullable
    }
}
