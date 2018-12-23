using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POC.Models
{
    [Table("AssignedProjects")]
    public class AssignedProjects
    {
        [Key]
        public int AssignedProjectID { get; set; }
        public List<ProjectMaster> Projects { get; set; }
        public int ManagerId { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }

    }
}
