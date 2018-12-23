using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace POC.ViewModels
{
    [NotMapped]
    public class AssignRolesModel
    {
        public List<AdminModel> ListofAdmins { get; set; }

        [Required(ErrorMessage = "Choose Manager")]
        public int RegistrationID { get; set; }
        public List<UserModel> ListofUser { get; set; }
        public int? AssignToAdmin { get; set; }
        public int? CreatedBy { get; set; }        
    }
    
}
