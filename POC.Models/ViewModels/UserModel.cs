using System.ComponentModel.DataAnnotations.Schema;

namespace POC.ViewModels
{
    [NotMapped]
    public class UserModel
    {
        public int RegistrationID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string AdminFirstName { get; set; }
        public string AdminMiddleName { get; set; }
        public string AdminLastName { get; set; }
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
        public bool selectedUsers { get; set; }
       // public string AssignToAdmin { get; set; }

        public string AssignToAdmin
        {
            get
            {
                string fullname = string.Empty;
                if (!string.IsNullOrEmpty(AdminFirstName))
                {

                    fullname = AdminFirstName.Trim();
                }
                if (!string.IsNullOrEmpty(AdminMiddleName))
                {
                    fullname = fullname + "," + AdminMiddleName.Trim();
                }
                if (!string.IsNullOrEmpty(AdminLastName))
                {
                    fullname = fullname + "," + AdminLastName.Trim();
                }
                return fullname;
            }
            set { }
        }

    }
}
