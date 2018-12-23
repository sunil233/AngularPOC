using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.Models
{
    [Table("Department")]
    public class DepartmentTB
    {
        [Key]
        public int DeptId { get; set; }
        [Required(ErrorMessage = "Enter Department Name")]
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
    }
}
