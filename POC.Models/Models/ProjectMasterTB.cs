using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POC.Models
{
    [Table("ProjectMaster")]
    public class ProjectMaster
    {
        [Key]
        public int ProjectID { get; set; }
        [Required(ErrorMessage = "Enter Project Code")]
        public string ProjectCode { get; set; }
        [Required(ErrorMessage = "Enter Nature of Industry")]
        public string NatureofIndustry { get; set; }
        [Required(ErrorMessage = "Enter ProjectName")]
        public string ProjectName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        

    }
}
