using POC.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace POC.ViewModels
{
    [NotMapped]
    public class RegistrationViewSummaryModel
    {
        public int RegistrationID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName
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
        public string Mobileno { get; set; }
        public string WorkEmail { get; set; }
        public string Username { get; set; }
        public string AssignToAdmin { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ValueDescription> Roles { get; set; }
        public List<ValueDescription> Departments { get; set; }
        public List<ValueDescription> Jobs { get; set; }
        public bool? IsActive { get; set; }

        public string EmployeeID { get; set; }

    }
}
