using System;
using System.ComponentModel.DataAnnotations;

namespace POC.ViewModels
{
    public class TimeSheetExcelExportModel
    {
        [Display(Name = "TimeSheet From Date")]
        [Required(ErrorMessage = "Please Choose From Date")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "TimeSheet To Date")]
        [Required(ErrorMessage = "Please Choose To Date")]
        public DateTime? ToDate { get; set; }
    }
}
