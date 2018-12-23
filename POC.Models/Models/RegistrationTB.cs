using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using POC.ViewModels;

namespace POC.Models
{
    [Table("Registration")]
    public class Registration
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter Last Name")]
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Mobileno Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobileno")]
        public string Mobileno { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string WorkEmail { get; set; }


        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in charaters")]
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }
        [MinLength(7, ErrorMessage = "Minimum Password must be 7 in charaters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Enter Birth Date")]
        public DateTime? Birthdate { get; set; }

        [Required(ErrorMessage = "Enter Date of Joining")]
        public DateTime? DateofJoining { get; set; }
        public DateTime? DateofLeaving { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public int? RoleID { get; set; }
        public bool? IsActive { get; set; }
        
        public string EmployeeID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ForceChangePassword { get; set; }

        [Required(ErrorMessage = "Department is Required")]
        public int? DeptId { get; set; }

        [Required(ErrorMessage = "Job Title is Required")]
        public int? JobId { get; set; }

        [NotMapped]
        public List<SelectListItem> Roles { get; set; }
        [NotMapped]
        public List<SelectListItem> Departments { get; set; }

        [NotMapped]
        public List<SelectListItem> Jobs { get; set; }

        [NotMapped]
        public List<SelectListItem> Managers { get; set; }

        public int? ManagerId { get; set; }

        public string EmergencyContact { get; set; }
        public string EmergencyContactNumber { get; set; }
    }
}
