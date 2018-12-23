using System;
using System.Collections.Generic;
using System.Data;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface ITimeSheetExportRepository
    {
        DataSet GetReportofTimeSheet(DateTime? FromDate, DateTime? ToDate, int UserID);
        dynamic GetWeekTimeSheetDetails(int TimeSheetMasterID);
        List<RegistrationViewSummaryModel> ListofEmployees(int UserID);
        List<int> GetTimeSheetMasterIDTimeSheet(DateTime? FromDate, DateTime? ToDate, int UserID);
        string GetUsernamebyRegistrationID(int RegistrationID);
        DataSet GetTimeSheetMasterIDTimeSheet(DateTime? FromDate, DateTime? ToDate);
    }
}
