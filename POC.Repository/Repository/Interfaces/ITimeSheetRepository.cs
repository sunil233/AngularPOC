using System;
using System.Collections.Generic;
using System.Linq;
using POC.Models;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface ITimeSheetRepository
    {
        int AddTimeSheetMaster(TimeSheetMaster TimeSheetMaster);
        int UpdateTimeSheetMaster(TimeSheetMaster TimeSheetMaster);
        int AddTimeSheetDetail(TimeSheetDetails TimeSheetDetails);
        bool CheckIsDateAlreadyUsed(DateTime FromDate, int UserID);
        IQueryable<TimeSheetMasterView> ShowTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID);
        List<TimeSheetDetailsView> TimesheetDetailsbyTimeSheetMasterID(int UserID, int TimeSheetMasterID);
        int DeleteTimesheetByTimeSheetMasterID(int TimeSheetMasterID, int UserID);
        IQueryable<TimeSheetMasterView> ShowAllTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID,int timesheetStatus);
        List<TimeSheetDetailsView> TimesheetDetailsbyTimeSheetMasterID(int TimeSheetMasterID);
        List<Periods> GetPeriodsbyTimeSheetMasterID(int TimeSheetMasterID);
        List<ProjectNames> GetProjectNamesbyTimeSheetMasterID(int TimeSheetMasterID);
        bool UpdateTimeSheetStatus(TimeSheetApproval timesheetapprovalmodel, int Status);
        void InsertTimeSheetAuditLog(TimeSheetAuditTB timesheetaudittb);
        int DeleteTimesheetByOnlyTimeSheetMasterID(int TimeSheetMasterID);
        int? InsertDescription(DescriptionTB DescriptionTB);
        DisplayViewModel GetTimeSheetsCountByAdminID(string AdminID);
        IQueryable<TimeSheetMasterView> ShowAllApprovedTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID);
        IQueryable<TimeSheetMasterView> ShowAllRejectTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID);
        IQueryable<TimeSheetMasterView> ShowAllSubmittedTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID);
        DisplayViewModel GetTimeSheetsCountByUserID(string UserID);
        IQueryable<TimeSheetMasterView> ShowTimeSheetStatus(string sortColumn, string sortColumnDir, string Search, int UserID, int TimeSheetStatus);
        bool UpdateTimeSheetAuditStatus(int TimeSheetMasterID, string Comment, int Status);
        bool IsTimesheetALreadyProcessed(int TimeSheetMasterID);     
        int GetTimesheetstatus(string TimeSheetMasterID);
        TimeSheetView GetTimeSheetMasterIDTimeSheet(DateTime? FromDate, DateTime? ToDate, int UserID);
        bool IsTimesheetAlreadyExist(int TimeSheetID);
        int UpdateTimeSheetDetail(TimeSheetDetails TimeSheetDetails);
        IQueryable<TimeSheetMasterView> ShowMyrecentSubmittedTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID);
        TimeSheetView GetTimeSheetByMasterId(int TimeSheetId);
    }
}
