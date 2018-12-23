using System;
using System.ComponentModel.DataAnnotations;


namespace POC.ViewModels
{
    public class TimeSheetExportUserModel
    {
        [Display(Name = "TimeSheet From Date")]
        [Required(ErrorMessage = "Please Choose From Date")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "TimeSheet To Date")]
        [Required(ErrorMessage = "Please Choose To Date")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Employee Name")]
        [Required(ErrorMessage = "Please Select Employee Name")]
        public int RegistrationID { get; set; }
    }
}
