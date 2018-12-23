
namespace POC.ViewModels
{
    public class TimeSheetExportModel
    {
        public int TimeSheetMasterID { get; set; }
        public int TotalHours { get; set; }
        public string Name { get; set; }
    }

    public class Periods
    {
        public string Period { get; set; }
    }

    public class ProjectNames
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
    }

    public class TimeSheetHours
    {
        public int Hours { get; set; }
        public int TimeSheetID { get; set; }
        public string comments { get; set; }
        public int TimesheetStatus { get; set; }
    }

}
