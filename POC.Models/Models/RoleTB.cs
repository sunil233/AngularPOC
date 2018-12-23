using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.Models
{

    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        [Required(ErrorMessage = "Enter Role Name")]
        public string Rolename { get; set; }
        public bool IsActive { get; set; }
        public string RoleCode { get; set; }
    }
   
}
