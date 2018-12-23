using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using POC.Repository.Interface;
using POC.ViewModels;

namespace POC.Repository.Implementation
{
    public class TimeSheetExportRepository : ITimeSheetExportRepository
    {
        public DataSet GetReportofTimeSheet(DateTime? FromDate, DateTime? ToDate, int UserID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Usp_GetReportofTimeSheet", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    cmd.Parameters.AddWithValue("@AssignTo", UserID);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public dynamic GetWeekTimeSheetDetails(int TimeSheetMasterID)
        {
            try
            {

                using (DatabaseContext _context = new DatabaseContext())
                {
                    var listTimeSheetDetails = (from tm in _context.TimeSheetDetails
                                                join pm in _context.ProjectMaster on tm.ProjectID equals pm.ProjectID
                                                select new
                                                {
                                                    tm.DaysofWeek,
                                                    tm.Hours,
                                                    tm.Period,
                                                    pm.ProjectName,
                                                    tm.CreatedOn
                                                }).ToList();
                    return listTimeSheetDetails;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<RegistrationViewSummaryModel> ListofEmployees(int UserID)
        {
            using (DatabaseContext _context = new DatabaseContext())
            {
                var listofemployee = (from registration in _context.Registration
                                      join AssignedRolesAdmin in _context.AssignedRoles on registration.RegistrationID equals AssignedRolesAdmin.RegistrationID
                                      where AssignedRolesAdmin.AssignToAdmin == UserID
                                      select new RegistrationViewSummaryModel()
                                      {
                                          FirstName = registration.FirstName,
                                          MiddleName = registration.MiddleName,
                                          LastName = registration.LastName,
                                          RegistrationID = registration.RegistrationID,
                                          Username = registration.Username
                                      }).ToList();
                return listofemployee;
            }
        }

        public List<int> GetTimeSheetMasterIDTimeSheet(DateTime? FromDate, DateTime? ToDate, int UserID)
        {
            try
            {

                using (DatabaseContext _context = new DatabaseContext())
                {
                    var listTimeSheetMasterID = (from tm in _context.TimeSheetMaster
                                                 where tm.UserID == UserID && (tm.FromDate >= FromDate && tm.ToDate <= ToDate)
                                                 select tm.TimeSheetMasterID).ToList();
                    return listTimeSheetMasterID;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetTimeSheetMasterIDTimeSheet(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Usp_GetTimeSheetbyFromDateandToDateTimeSheet", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string GetUsernamebyRegistrationID(int RegistrationID)
        {
            using (DatabaseContext _context = new DatabaseContext())
            {
                var employee = (from registration in _context.Registration
                                where registration.RegistrationID == RegistrationID
                                select registration).FirstOrDefault();
                if (employee != null)
                    return TimeSheetHelper.GetFullName(employee.FirstName, employee.MiddleName, employee.LastName);
                else
                    return "";
            }


        }
    }
}
