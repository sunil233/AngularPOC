using System.Collections.Generic;
namespace POC.ViewModels
{
    public class AssignProjects
    {
        public List<ProjectMasterViewModel> Projects { get; set; }
        public List<UserModel> Users { get; set; }
        public int ManagerId { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
    }
}
