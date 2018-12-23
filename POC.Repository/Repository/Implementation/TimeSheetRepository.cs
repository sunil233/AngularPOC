using System;
using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
using System.Linq.Dynamic;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data.Entity;
using POC.ViewModels;
using System.Globalization;

namespace POC.Repository.Implementation
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        public int AddTimeSheetMaster(TimeSheetMaster TimeSheetMaster)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.TimeSheetMaster.Add(TimeSheetMaster);
                    _context.SaveChanges();
                    int id = TimeSheetMaster.TimeSheetMasterID; // Yes it's here
                    return id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateTimeSheetMaster(TimeSheetMaster TimeSheetMaster)
        {
            var result = 0;
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var tsentity = (from ts in _context.TimeSheetDetails.Where(x => x.TimeSheetMasterID == TimeSheetMaster.TimeSheetMasterID)
                                    select ts).FirstOrDefault();
                    if (tsentity != null)
                    {
                        tsentity.TimesheetStatus = TimeSheetMaster.TimeSheetStatus;
                        tsentity.Hours = TimeSheetMaster.TotalHours;
                        _context.Entry(tsentity).State = EntityState.Modified;
                    }
                    result = _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public int AddTimeSheetDetail(TimeSheetDetails TimeSheetDetails)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.TimeSheetDetails.Add(TimeSheetDetails);
                    _context.SaveChanges();
                    int id = TimeSheetDetails.TimeSheetID; // Yes it's here
                    return id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateTimeSheetDetail(TimeSheetDetails TimeSheetDetails)
        {
            int result = 0;
            using (var _context = new DatabaseContext())
            {
                var timesheetdetails = (from timesheet in _context.TimeSheetDetails.Where(x => x.TimeSheetID == TimeSheetDetails.TimeSheetID)
                                        select timesheet).FirstOrDefault();
                if (timesheetdetails != null)
                {
                    timesheetdetails.Hours = TimeSheetDetails.Hours;
                    _context.Entry(timesheetdetails).State = EntityState.Modified;
                    result = _context.SaveChanges();
                }
            }
            return result;
        }


        public bool CheckIsDateAlreadyUsed(DateTime FromDate, int UserID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from timesheetdetails in _context.TimeSheetDetails
                                  where timesheetdetails.Period == FromDate && timesheetdetails.UserID == UserID
                                  select timesheetdetails).Count();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<TimeSheetMasterView> ShowTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster

                                       where timesheetmaster.UserID == UserID
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = SqlFunctions.DateName("day", timesheetmaster.FromDate).Trim() + "/" +
                                                      SqlFunctions.StringConvert((double)timesheetmaster.FromDate.Value.Month).TrimStart() + "/" +
                                                      SqlFunctions.DateName("year", timesheetmaster.FromDate),
                                           ToDate = SqlFunctions.DateName("day", timesheetmaster.ToDate).Trim() + "/" +
                                                      SqlFunctions.StringConvert((double)timesheetmaster.ToDate.Value.Month).TrimStart() + "/" +
                                                      SqlFunctions.DateName("year", timesheetmaster.ToDate),

                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" +
                                                      SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                      SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate == Search);
            }

            return IQueryabletimesheet;

        }

        public List<TimeSheetDetailsView> TimesheetDetailsbyTimeSheetMasterID(int UserID, int TimeSheetMasterID)
        {
            using (var _context = new DatabaseContext())
            {


                var data = (from timesheet in _context.TimeSheetDetails
                            join project in _context.ProjectMaster on timesheet.ProjectID equals project.ProjectID
                            where timesheet.UserID == UserID && timesheet.TimeSheetMasterID == TimeSheetMasterID
                            select new TimeSheetDetailsView
                            {
                                TimeSheetID = timesheet.TimeSheetID,
                                CreatedOn = SqlFunctions.DateName("day", timesheet.CreatedOn).Trim() + "/" +
                    SqlFunctions.StringConvert((double)timesheet.CreatedOn.Value.Month).TrimStart() + "/" +
                    SqlFunctions.DateName("year", timesheet.CreatedOn),
                                Period = SqlFunctions.DateName("day", timesheet.Period).Trim() + "/" +
                    SqlFunctions.StringConvert((double)timesheet.Period.Value.Month).TrimStart() + "/" +
                    SqlFunctions.DateName("year", timesheet.Period),
                                DaysofWeek = timesheet.DaysofWeek,
                                Hours = timesheet.Hours,
                                ProjectName = project.ProjectName,
                                TimeSheetMasterID = timesheet.TimeSheetMasterID

                            }).ToList();

                return data;
            }
        }

        public List<TimeSheetDetailsView> TimesheetDetailsbyTimeSheetMasterID(int TimeSheetMasterID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from timesheet in _context.TimeSheetDetails
                            join project in _context.ProjectMaster on timesheet.ProjectID equals project.ProjectID
                            where timesheet.TimeSheetMasterID == TimeSheetMasterID
                            select new TimeSheetDetailsView
                            {
                                TimeSheetID = timesheet.TimeSheetID,
                                CreatedOn = SqlFunctions.DateName("day", timesheet.CreatedOn).Trim() + "/" +
                                            SqlFunctions.StringConvert((double)timesheet.CreatedOn.Value.Month).TrimStart() + "/" +
                                            SqlFunctions.DateName("year", timesheet.CreatedOn),
                                Period = SqlFunctions.DateName("day", timesheet.Period).Trim() + "/" +
                                            SqlFunctions.StringConvert((double)timesheet.Period.Value.Month).TrimStart() + "/" +
                                            SqlFunctions.DateName("year", timesheet.Period),
                                DaysofWeek = timesheet.DaysofWeek,
                                Hours = timesheet.Hours,
                                ProjectName = project.ProjectName,
                                TimeSheetMasterID = timesheet.TimeSheetMasterID

                            }).ToList();

                return data;
            }
        }

        public int DeleteTimesheetByTimeSheetMasterID(int TimeSheetMasterID, int UserID)
        {
            try
            {

                var result = 0;
                using (var _context = new DatabaseContext())
                {
                    //delete TimeSheetMaster data
                    var timesheetmasterentity = (from timesheetmaster in _context.TimeSheetMaster.Where(x => x.TimeSheetMasterID == TimeSheetMasterID)
                                                 select timesheetmaster).FirstOrDefault();
                    if (timesheetmasterentity != null)
                    {
                        _context.TimeSheetMaster.Remove(timesheetmasterentity);
                        _context.SaveChanges();
                        //delete TimeSheetDetails data
                        var timesheetdetailsentity = (from timesheetdetails in _context.TimeSheetDetails.Where(x => x.TimeSheetMasterID == TimeSheetMasterID && x.UserID == UserID)
                                                      select timesheetdetails).FirstOrDefault();
                        if (timesheetdetailsentity != null)
                        {
                            _context.TimeSheetDetails.Remove(timesheetdetailsentity);
                            _context.SaveChanges();
                        }
                        //delete TimeSheetAuditTB data
                        var timesheetAuditdetailsentity = (from timesheetAuditdetails in _context.TimeSheetAuditTB.Where(x => x.TimeSheetMasterID == TimeSheetMasterID && x.UserID == UserID)
                                                           select timesheetAuditdetails).FirstOrDefault();
                        if (timesheetAuditdetailsentity != null)
                        {
                            _context.TimeSheetAuditTB.Remove(timesheetAuditdetailsentity);
                            result = _context.SaveChanges();
                        }

                    }
                    return result;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<TimeSheetMasterView> ShowAllTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID,int timesheetStatus=0)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join registration in _context.Registration on timesheetmaster.UserID equals registration.RegistrationID
                                       join AssignedRolesAdmin in _context.AssignedRoles on registration.RegistrationID equals AssignedRolesAdmin.RegistrationID
                                       where AssignedRolesAdmin.AssignToAdmin == UserID && timesheetmaster.TimeSheetStatus== timesheetStatus
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.FromDate)), 4) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.FromDate))), 2) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.FromDate))), 2)).Replace(" ", "0"),
                                           ToDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.ToDate)), 4) + "-"
                                                        + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.ToDate))), 2) + "-"
                                                        + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.ToDate))), 2)).Replace(" ", "0"),

                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours,
                                           Username = registration.Username,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.ToDate).ToString(),
                                           FirstName = registration.FirstName,
                                           LastName = registration.LastName,
                                           MiddleName = registration.MiddleName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate.Contains(Search) || m.Username.Contains(Search));
            }

            return IQueryabletimesheet;

        }

        public List<Periods> GetPeriodsbyTimeSheetMasterID(int TimeSheetMasterID)
        {
            try
            {
                var periodList = new List<Periods>();
                using (var _context = new DatabaseContext())
                {
                    var groupedData = (from T in _context.TimeSheetDetails
                                       join TA in _context.TimeSheetAuditTB on T.TimeSheetMasterID equals TA.TimeSheetMasterID
                                       select T).ToList().Where(x => x.TimeSheetMasterID == TimeSheetMasterID).GroupBy(x => x.Period);
                    if (groupedData != null)
                    {
                        groupedData.ToList().ForEach(u =>
                        {
                            if (u.Key != null)
                            {
                                var period = Convert.ToDateTime(u.Key, new CultureInfo("es-US", true)).ToString("MM/dd/yyyy");
                                periodList.Add(new Periods { Period = period });
                            }
                        });
                    }
                    return periodList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProjectNames> GetProjectNamesbyTimeSheetMasterID(int TimeSheetMasterID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var projectlist = (from TM in _context.TimeSheetDetails
                                       join PM in _context.ProjectMaster on TM.ProjectID equals PM.ProjectID
                                       where TM.TimeSheetMasterID == TimeSheetMasterID
                                       group new { TM, PM } by new
                                       {
                                           TM.ProjectID,
                                           PM.ProjectName,
                                           PM.ProjectCode
                                       } into g
                                       select new ProjectNames
                                       {
                                           ProjectID = (int)g.Key.ProjectID,
                                           ProjectName = g.Key.ProjectName,
                                           ProjectCode = g.Key.ProjectCode
                                       }).ToList();
                    return projectlist;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateTimeSheetStatus(TimeSheetApproval timesheetapprovalmodel, int Status)
        {
            var result = 0;
            using (var _context = new DatabaseContext())
            {
                var tmentity = (from tm in _context.TimeSheetMaster.Where(x => x.TimeSheetMasterID == timesheetapprovalmodel.TimeSheetMasterID)
                                select tm).FirstOrDefault();
                if (tmentity != null)
                {
                    tmentity.TimeSheetStatus = Status;
                    tmentity.Comment = timesheetapprovalmodel.Comment;
                    _context.Entry(tmentity).State = EntityState.Modified;
                    _context.SaveChanges();
                    //
                    var tsentity = (from ts in _context.TimeSheetDetails.Where(x => x.TimeSheetMasterID == timesheetapprovalmodel.TimeSheetMasterID)
                                    select ts).FirstOrDefault();
                    if (tsentity != null)
                    {
                        tsentity.TimesheetStatus = Status;
                        _context.Entry(tsentity).State = EntityState.Modified;
                    }
                    result = _context.SaveChanges();
                }
            }
            if (result > 0)
                return true;
            else
                return false;
        }

        public void InsertTimeSheetAuditLog(TimeSheetAuditTB timesheetaudittb)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.TimeSheetAuditTB.Add(timesheetaudittb);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteTimesheetByOnlyTimeSheetMasterID(int TimeSheetMasterID)
        {
            int resultTimeSheetMaster = 0;
            int resultTimeSheetDetails = 0;
            try
            {
                using (var _context = new DatabaseContext())
                {

                    var timesheetcount = (from ex in _context.TimeSheetMaster
                                          where ex.TimeSheetMasterID == TimeSheetMasterID
                                          select ex).Count();

                    if (timesheetcount > 0)
                    {
                        TimeSheetMaster timesheet = (from ex in _context.TimeSheetMaster
                                                     where ex.TimeSheetMasterID == TimeSheetMasterID
                                                     select ex).SingleOrDefault();

                        _context.TimeSheetMaster.Remove(timesheet);
                        resultTimeSheetMaster = _context.SaveChanges();
                    }

                    var timesheetdetailscount = (from ex in _context.TimeSheetDetails
                                                 where ex.TimeSheetMasterID == TimeSheetMasterID
                                                 select ex).Count();

                    if (timesheetdetailscount > 0)
                    {

                        var timesheetdetails = (from ex in _context.TimeSheetDetails
                                                where ex.TimeSheetMasterID == TimeSheetMasterID
                                                select ex).ToList();

                        _context.TimeSheetDetails.RemoveRange(timesheetdetails);
                        resultTimeSheetDetails = _context.SaveChanges();

                    }

                    if (resultTimeSheetMaster > 0 || resultTimeSheetDetails > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? InsertDescription(DescriptionTB DescriptionTB)
        {
            using (var _context = new DatabaseContext())
            {
                _context.DescriptionTB.Add(DescriptionTB);
                _context.SaveChanges();
                int? id = DescriptionTB.DescriptionID; // Yes it's here
                return id;
            }
        }

        public DisplayViewModel GetTimeSheetsCountByAdminID(string AdminID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var adminId = Convert.ToInt32(AdminID);
                    var groupedData = _context.TimeSheetAuditTB.Where(x => x.ApprovalUser == adminId)
                        .GroupBy(x => x.ApprovalUser).Select(g => new DisplayViewModel
                        {
                            ApprovalUser = g.Key,
                            SaveddCount = g.Count(p => (p.Status == 1 ? (System.Int64?)1 : (System.Int64?)null) != null),
                            ApprovedCount = g.Count(p => (p.Status == 2 ? (System.Int64?)1 : (System.Int64?)null) != null),
                            RejectedCount = g.Count(p => (p.Status == 3 ? (System.Int64?)1 : (System.Int64?)null) != null),
                            SubmittedCount = g.Count(p => (p.Status == 4 ? (System.Int64?)1 : (System.Int64?)null) != null)
                        }).SingleOrDefault();
                    return groupedData;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public IQueryable<TimeSheetMasterView> ShowAllApprovedTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join timeSheetAuditTB in _context.TimeSheetAuditTB on timesheetmaster.TimeSheetMasterID equals timeSheetAuditTB.TimeSheetMasterID
                                       join registration in _context.Registration on timesheetmaster.UserID equals registration.RegistrationID
                                       join AssignedRolesAdmin in _context.AssignedRoles on registration.RegistrationID equals AssignedRolesAdmin.RegistrationID
                                       where AssignedRolesAdmin.AssignToAdmin == UserID && timeSheetAuditTB.Status == 2
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.FromDate)), 4) + "-"
                                                     + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.FromDate))), 2) + "-"
                                                     + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.FromDate))), 2)).Replace(" ", "0"),
                                           ToDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.ToDate)), 4) + "-"
                                                        + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.ToDate))), 2) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.ToDate))), 2)).Replace(" ", "0"),
                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" + SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours,
                                           Username = registration.Username,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.ToDate).ToString(),
                                           FirstName = registration.FirstName,
                                           LastName = registration.LastName,
                                           MiddleName = registration.MiddleName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate == Search || m.Username == Search);
            }

            return IQueryabletimesheet;

        }

        public IQueryable<TimeSheetMasterView> ShowAllRejectTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join timeSheetAuditTB in _context.TimeSheetAuditTB on timesheetmaster.TimeSheetMasterID equals timeSheetAuditTB.TimeSheetMasterID
                                       join registration in _context.Registration on timesheetmaster.UserID equals registration.RegistrationID
                                       join AssignedRolesAdmin in _context.AssignedRoles on registration.RegistrationID equals AssignedRolesAdmin.RegistrationID
                                       where AssignedRolesAdmin.AssignToAdmin == UserID && timeSheetAuditTB.Status == 3
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.FromDate)), 4) + "-"
                                                     + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.FromDate))), 2) + "-"
                                                     + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.FromDate))), 2)).Replace(" ", "0"),
                                           ToDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.ToDate)), 4) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.ToDate))), 2) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.ToDate))), 2)).Replace(" ", "0"),
                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours,
                                           Username = registration.Username,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.ToDate).ToString(),
                                           FirstName = registration.FirstName,
                                           LastName = registration.LastName,
                                           MiddleName = registration.MiddleName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate == Search || m.Username == Search);
            }

            return IQueryabletimesheet;

        }

        public IQueryable<TimeSheetMasterView> ShowAllSubmittedTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join timeSheetAuditTB in _context.TimeSheetAuditTB on timesheetmaster.TimeSheetMasterID equals timeSheetAuditTB.TimeSheetMasterID
                                       join registration in _context.Registration on timesheetmaster.UserID equals registration.RegistrationID
                                       join AssignedRolesAdmin in _context.AssignedRoles on registration.RegistrationID equals AssignedRolesAdmin.RegistrationID
                                       where AssignedRolesAdmin.AssignToAdmin == UserID && timeSheetAuditTB.Status == 4
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.FromDate)), 4) + "-"
                                                        + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.FromDate))), 2) + "-"
                                                        + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.FromDate))), 2)).Replace(" ", "0"),
                                           ToDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.ToDate)), 4) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.ToDate))), 2) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.ToDate))), 2)).Replace(" ", "0"),
                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours,
                                           Username = registration.Username,
                                           FirstName = registration.FirstName,
                                           LastName = registration.LastName,
                                           MiddleName = registration.MiddleName,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.ToDate).ToString()
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate == Search || m.Username == Search);
            }

            return IQueryabletimesheet;

        }


        public IQueryable<TimeSheetMasterView> ShowMyrecentSubmittedTimeSheet(string sortColumn, string sortColumnDir, string Search, int UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join timeSheetAuditTB in _context.TimeSheetAuditTB on timesheetmaster.TimeSheetMasterID equals timeSheetAuditTB.TimeSheetMasterID
                                       join registration in _context.Registration on timesheetmaster.UserID equals registration.RegistrationID
                                       join AssignedRolesAdmin in _context.AssignedRoles on registration.RegistrationID equals AssignedRolesAdmin.RegistrationID
                                       where registration.RegistrationID == UserID
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.FromDate)), 4) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.FromDate))), 2) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.FromDate))), 2)).Replace(" ", "0"),
                                           ToDate = (DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.ToDate)), 4) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.ToDate))), 2) + "-"
                                                    + DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.ToDate))), 2)).Replace(" ", "0"),
                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours,
                                           Username = registration.Username,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.ToDate).ToString(),
                                           FirstName = registration.FirstName,
                                           LastName = registration.LastName,
                                           MiddleName = registration.MiddleName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate == Search || m.Username == Search);
            }

            return IQueryabletimesheet;

        }
        public DisplayViewModel GetTimeSheetsCountByUserID(string UserID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var userId = Convert.ToInt32(UserID);
                    var groupedData = _context.TimeSheetAuditTB.Where(x => x.UserID == userId)
                        .GroupBy(x => x.UserID).Select(g => new DisplayViewModel
                        {
                            ApprovalUser = g.Key,
                            SaveddCount = g.Count(p => (p.Status == 1 ? 1 : (System.Int64?)null) != null),
                            ApprovedCount = g.Count(p => (p.Status == 2 ? 1 : (System.Int64?)null) != null),
                            RejectedCount = g.Count(p => (p.Status == 3 ? 1 : (System.Int64?)null) != null),
                            SubmittedCount = g.Count(p => (p.Status == 4 ? 1 : (System.Int64?)null) != null)
                        }).FirstOrDefault();
                    return groupedData;
                }

            }
            catch (Exception)
            {
                return new DisplayViewModel();
            }


        }

        public IQueryable<TimeSheetMasterView> ShowTimeSheetStatus(string sortColumn, string sortColumnDir, string Search, int UserID, int TimeSheetStatus)
        {
            var _context = new DatabaseContext();
            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join registration in _context.Registration on timesheetmaster.UserID equals registration.RegistrationID
                                       where timesheetmaster.UserID == UserID && timesheetmaster.TimeSheetStatus == TimeSheetStatus
                                       select new TimeSheetMasterView
                                       {
                                           TimeSheetStatus = timesheetmaster.TimeSheetStatus == 1 ? "Saved" : timesheetmaster.TimeSheetStatus == 4 ? "Submitted" : timesheetmaster.TimeSheetStatus == 2 ? "Approved" : "Rejected",
                                           Comment = timesheetmaster.Comment,
                                           TimeSheetMasterID = timesheetmaster.TimeSheetMasterID,
                                           FromDate = SqlFunctions.DateName("day", timesheetmaster.FromDate).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.FromDate.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.FromDate),
                                           ToDate = SqlFunctions.DateName("day", timesheetmaster.ToDate).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.ToDate.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.ToDate),
                                           CreatedOn = SqlFunctions.DateName("day", timesheetmaster.CreatedOn).Trim() + "/" +
                                                       SqlFunctions.StringConvert((double)timesheetmaster.CreatedOn.Value.Month).TrimStart() + "/" +
                                                       SqlFunctions.DateName("year", timesheetmaster.CreatedOn),
                                           TotalHours = timesheetmaster.TotalHours,
                                           Username = registration.Username,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.ToDate).ToString(),
                                           FirstName = registration.FirstName,
                                           LastName = registration.LastName,
                                           MiddleName = registration.MiddleName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FromDate == Search);
            }

            return IQueryabletimesheet;

        }

        public bool UpdateTimeSheetAuditStatus(int TimeSheetMasterID, string Comment, int Status)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                SqlTransaction sql = con.BeginTransaction();

                try
                {
                    var param = new DynamicParameters();
                    param.Add("@TimeSheetMasterID", TimeSheetMasterID);
                    param.Add("@Comment", Comment);
                    param.Add("@Status", Status);
                    var result = con.Execute("Usp_ChangeTimesheetStatus", param, sql, 0, System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        sql.Commit();
                        return true;
                    }
                    else
                    {
                        sql.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {
                    sql.Rollback();
                    throw;
                }
            }
        }

        public bool IsTimesheetALreadyProcessed(int TimeSheetMasterID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from timesheet in _context.TimeSheetAuditTB
                            where timesheet.TimeSheetMasterID == TimeSheetMasterID && timesheet.Status != 1
                            select timesheet).Count();

                if (data > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public int GetTimesheetstatus(string TimeSheetMasterID)
        {
            var i = Convert.ToInt64(TimeSheetMasterID);
            using (var _context = new DatabaseContext())
            {
                var timesheetstatus = (from p in _context.TimeSheetMaster
                                       where p.TimeSheetMasterID == i
                                       select p.TimeSheetStatus).FirstOrDefault();
                return timesheetstatus;
            }
        }

        public TimeSheetView GetTimeSheetMasterIDTimeSheet(DateTime? FromDate, DateTime? ToDate, int UserID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", FromDate);
                    param.Add("@ToDate", ToDate);
                    param.Add("@UserID", UserID);
                    var result = con.Query<TimeSheetDetailsView>("Usp_GetWeekTimeSheetDetailsByDateRange", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    var listtimesheetdetails = new List<List<TimeSheetDetailsView>>();
                    var rows = (from row in result
                                select new
                                {
                                    rowNo = row.RowNo,
                                }).Distinct().ToList();
                    foreach (var row in rows)
                    {
                        int rowNo = Convert.ToInt32(row.rowNo);
                        var timesheetdetails = result.Where(x => x.RowNo == rowNo).ToList();
                        listtimesheetdetails.Add(timesheetdetails);
                    }
                    if (result.Count > 0)
                    {
                        var timesheetView = new TimeSheetView()
                        {
                            ListTimeSheetDetails = listtimesheetdetails,
                            TimeSheetMasterID = result[0].TimeSheetMasterID,                          
                            TimeSheetStatus = GetTimesheetstatus(Convert.ToString(result[0].TimeSheetMasterID))
                        };
                        return timesheetView;
                    }
                    else
                    {
                        return new TimeSheetView();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public TimeSheetView GetTimeSheetByMasterId(int TimeSheetMasterID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@TimeSheetMasterID", TimeSheetMasterID);
                    var result = con.Query<TimeSheetDetailsView>("Usp_GetTimeSheetDetailsByID", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    var listtimesheetdetails = new List<List<TimeSheetDetailsView>>();
                    var rows = (from row in result
                                select new
                                {
                                    rowNo = row.RowNo,
                                }).Distinct().ToList();
                    foreach (var row in rows)
                    {
                        int rowNo = Convert.ToInt32(row.rowNo);
                        var timesheetdetails = result.Where(x => x.RowNo == rowNo).ToList();
                        listtimesheetdetails.Add(timesheetdetails);
                    }
                    if (result.Count > 0)
                    {
                        var timesheetView = new TimeSheetView()
                        {
                            ListTimeSheetDetails = listtimesheetdetails,
                            TimeSheetMasterID = result[0].TimeSheetMasterID,                            
                            TimeSheetStatus = GetTimesheetstatus(Convert.ToString(result[0].TimeSheetMasterID))
                        };
                        return timesheetView;
                    }
                    else
                    {
                        return new TimeSheetView();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public bool IsTimesheetAlreadyExist(int TimeSheetID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from timesheet in _context.TimeSheetDetails
                            where timesheet.TimeSheetID == TimeSheetID
                            select timesheet).Count();

                if (data > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
