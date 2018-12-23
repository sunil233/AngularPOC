using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.Models
{
    [Table("Task")]
    public class TaskTB
    {
        [Key]
        public int TaskID { get; set; }
        [Required(ErrorMessage = "Enter Task Name")]
        public string Taskname { get; set; }
        public int AssignedtoID { get; set; }
        public int ProjectID { get; set; }
        public string Comments { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int Status { get; set; }

    }
}
