using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printers.api.Models
{
    [Table("Designation")]
    public class Designation
    {
        [Key]
        public int DesignationID { get; set; }

        [Required]
        [StringLength(50)]
        public string DesignationName { get; set; }

        // Navigation Property
      //  public ICollection<Users> Users { get; set; }
    }
}
