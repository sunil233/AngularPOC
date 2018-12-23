namespace POC.ViewModels
{
    public class ProjectMasterViewModel
    {
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public bool selectedProject { get; set; }
        public int ManagerId { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ManagerName
        {
            get
            {
                string fullname = string.Empty;
                if (!string.IsNullOrEmpty(FirstName))
                {

                    fullname = FirstName.Trim();
                }
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    fullname = fullname + "," + MiddleName.Trim();
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    fullname = fullname + "," + LastName.Trim();
                }
                if(!string.IsNullOrEmpty(fullname))
                return fullname+"("+ RoleName+")";
                else
                    return fullname;
            }
            set { }
        }
    }
}
