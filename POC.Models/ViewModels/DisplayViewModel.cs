using System.ComponentModel.DataAnnotations.Schema;


namespace POC.ViewModels
{
    [NotMapped]
    public class DisplayViewModel
    {
        public int ApprovalUser { get; set; }
        public int SubmittedCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int SaveddCount { get; set; }
        
    }
}
