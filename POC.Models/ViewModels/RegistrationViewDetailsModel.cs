using POC.Models;
using System.Collections.Generic;



namespace POC.ViewModels
{
    public class RegistrationViewDetailsModel
    {
        public string EmployeeID { get; set; }
        public int RegistrationID { get; set; }
       
       
        public string Mobileno { get; set; }

         public string EmailID { get; set; }

         public string WorkEmail { get; set; }
        public string Username { get; set; }
      
        public string Birthdate { get; set; }
       
        public string DateofJoining { get; set; }
       
        public string Gender { get; set; }
        
        public int RoleID { get; set; }
      

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

     
        public int? DeptId { get; set; }

     
        public int? JobId { get; set; }
        public string DateofLeaving { get; set; }
        public bool IsActive { get; set; }
        public List<ValueDescription> Roles { get; set; }
        public List<ValueDescription> Departments { get; set; }
        public List<ValueDescription> Jobs { get; set; }      
        public List<ValueDescription> Managers { get; set; }

        public int? ManagerId { get; set; }
       
        public string EmergencyContact { get; set; }
        public string EmergencyContactNumber { get; set; }
       
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

        public string JobTitle { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }
      
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerMiddleName { get; set; }
        public string ManagerFullName
        {
            get
            {
                string fullname = string.Empty;
                if (!string.IsNullOrEmpty(ManagerFirstName))
                {

                    fullname = ManagerFirstName.Trim();
                }
                if (!string.IsNullOrEmpty(ManagerMiddleName))
                {
                    fullname = fullname + "," + ManagerMiddleName.Trim();
                }
                if (!string.IsNullOrEmpty(ManagerLastName))
                {
                    fullname = fullname + "," + ManagerLastName.Trim();
                }
                return fullname;
            }
            set { }
        }

    }
}
