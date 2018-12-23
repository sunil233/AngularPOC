using System.ComponentModel.DataAnnotations.Schema;


namespace POC.ViewModels
{
    [NotMapped]
    public class TimeSheetMasterView
    {
        public int TimeSheetMasterID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? TotalHours { get; set; }
        public int UserID { get; set; }
        public string CreatedOn { get; set; }
        public string Username { get; set; }
        public string SubmittedMonth { get; set; }
        public string TimeSheetStatus { get; set; }
        public string Comment { get; set; }

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
    }
    public enum TimeSheetStatus
    {
        Save = 1,
        Approve = 2,
        Reject = 3,
        Submit = 4
    }
}



