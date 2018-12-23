using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POC.ViewModels
{
    [NotMapped]
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        [NotMapped]
        public string RoleCode { get; set; }

        [NotMapped]
        public bool IsAssigned { get; set; }

        [NotMapped]
        public int UserId { get; set; }
    }

    public enum RoleCode
    {
        [Description("SuperAdmin")]
        SuperAdmin,
        [Description("Manager")]
        Manager,
        [Description("DotNetDeveloper")]
        DotNetDeveloper,
        [Description("HR")]
        HR,
        [Description("Consultant")]
        Consultant,
        [Description("UIDeveloper")]
        UIDeveloper
    }

}
