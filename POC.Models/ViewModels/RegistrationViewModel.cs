using POC.Models;
namespace POC.ViewModels
{
    public class RegistrationViewModel
    {
        public Registration registration { get; set; }
        public Roles roles { get; set; }
        public DepartmentTB departments { get; set; }
        public JobsTB jobs { get; set; }           
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerMiddleName { get; set; }

       
    }
}
