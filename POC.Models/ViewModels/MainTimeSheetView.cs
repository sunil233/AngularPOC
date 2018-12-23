using System.Collections.Generic;

namespace POC.ViewModels
{
    public class MainTimeSheetView
    {
        public List<TimeSheetDetailsView> ListTimeSheetDetails { get; set; }
        public List<Periods> ListofPeriods { get; set; }
        public List<ProjectNames> ListofProjectNames { get; set; }
        public List<string> ListoDayofWeek { get; set; }
        public int TimeSheetMasterID { get; set; }
        public int TimeSheetID { get; set; }
        public int TimesheetStatus { get; set; }
    }
}
