using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printers.api
{
    [Table("PrinterMake")]
    public class PrinterMake
    {
        [Key]
        public int PrinterMakeID { get; set; }

        [Required]
        [StringLength(30)]
        public string PrinterMakeName { get; set; }

    }
}
