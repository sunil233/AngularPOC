using System.Collections.Generic;

namespace POC.ViewModels
{
    public class TimeSheetView
    {
        public List<List<TimeSheetDetailsView>> ListTimeSheetDetails { get; set; }

        public List<int> Rows { get; set; }
        public int TimeSheetMasterID { get; set; }
        public int TimeSheetStatus { get; set; }
    }
}
