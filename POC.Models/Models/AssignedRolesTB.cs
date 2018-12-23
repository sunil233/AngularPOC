using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POC.Models
{
    [Table("AssignedRoles")]
    public class AssignedRoles
    {
        [Key]
        public int AssignedRolesID { get; set; }
        public int? AssignToAdmin { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int RegistrationID { get; set; }
        public string Status { get; set; }

    }
}
