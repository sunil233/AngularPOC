using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace POC.Models
{
    [Table("TimeSheetAuditTB")]
    public class TimeSheetAuditTB
    {
        [Key]
        public int ApprovalTimeSheetLogID { get; set; }
        public int ApprovalUser { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public int TimeSheetMasterID { get; set; }
        public int UserID { get; set; }
    }

    [Table("ExpenseAuditTB")]
    public class ExpenseAuditTB
    {
        [Key]
        public int ApprovaExpenselLogID { get; set; }
        public int ApprovalUser { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public int ExpenseID { get; set; }
        public int UserID { get; set; }
    }



}
