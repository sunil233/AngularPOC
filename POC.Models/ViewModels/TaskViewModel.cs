namespace POC.ViewModels
{
    public class TaskViewModel
    {

        public int TaskID { get; set; }        
        public string Taskname { get; set; }
        public int AssignedtoID { get; set; }
        public int ProjectID { get; set; }
        public string Comments { get; set; }
        public string ProjectName { get; set; }      
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int Status { get; set; }
        public string StatusType { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string AssignedtoUser
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
                return fullname;
            }
            set { }
        }
    }
}
