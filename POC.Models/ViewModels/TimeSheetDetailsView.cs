using System.Collections.Generic;

namespace POC.ViewModels
{
    public class TimeSheetDetailsView
    {
        public int TimeSheetID { get; set; }
        public string DaysofWeek { get; set; }
        public int? Hours { get; set; }
        public string Period { get; set; }
        public int? ProjectID { get; set; }
        public int UserID { get; set; }
        public string CreatedOn { get; set; }
        public int TimeSheetMasterID { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string ProjectCode { get; set; }
        public int TimesheetStatus { get; set; }      
        public int TaskID { get; set; }
        public int RowNo { get; set; }       

    }

}
