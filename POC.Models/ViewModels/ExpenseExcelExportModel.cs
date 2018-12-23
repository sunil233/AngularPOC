using System;
using System.ComponentModel.DataAnnotations;

namespace POC.ViewModels
{
    public class ExpenseExcelExportModel
    {
        [Display(Name = "Expense From Date")]
        [Required(ErrorMessage = "Please Choose From Date")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "Expense To Date")]
        [Required(ErrorMessage = "Please Choose To Date")]
        public DateTime? ToDate { get; set; }
    }
}
