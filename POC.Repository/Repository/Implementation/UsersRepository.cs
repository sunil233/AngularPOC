using System;
using System.Linq;
using POC.Models;
using System.Linq.Dynamic;
using POC.Repository.Interface;
using POC.ViewModels;
using System.Globalization;
using System.Collections.Generic;

namespace POC.Repository.Implementation
{
    public class UsersRepository : IUsersRepository
    {
        public RegistrationViewDetailsModel GetAdminDetailsByRegistrationID(int? RegistrationID)
        {
            using (var _context = new DatabaseContext())
            {
                string RoleCode = "Manager";
                var queryResult = (from registration in _context.Registration
                                   join role in _context.Role on registration.RoleID equals role.RoleID
                                   where registration.RegistrationID == RegistrationID && role.RoleCode == RoleCode
                                   select registration).FirstOrDefault();
                if (queryResult != null)
                {
                    var objRegistrationViewDetailsModel = new RegistrationViewDetailsModel()
                    {
                        Birthdate = Convert.ToString(queryResult.Birthdate),
                        DateofLeaving = Convert.ToString(queryResult.DateofLeaving),
                        DateofJoining = Convert.ToString(queryResult.DateofLeaving),
                        EmailID = queryResult.EmailID,
                        EmployeeID = queryResult.EmployeeID,
                        Gender = queryResult.Gender,
                        IsActive = Convert.ToBoolean(queryResult.IsActive),
                        Mobileno = queryResult.Mobileno,
                        FullName = TimeSheetHelper.GetFullName(queryResult.FirstName, queryResult.MiddleName, queryResult.LastName),
                        RegistrationID = queryResult.RegistrationID,
                        RoleID = Convert.ToInt32(queryResult.RoleID),
                        Username = queryResult.Username
                    };
                    return objRegistrationViewDetailsModel;
                }
                return new RegistrationViewDetailsModel();
            }
        }

        public int GetAdminIDbyUserID(int UserID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from tm in _context.AssignedRoles
                            where tm.RegistrationID == UserID
                            select tm.AssignToAdmin).FirstOrDefault();
                if (data != null)
                    return Convert.ToInt32(data);
                else
                    return 0;
            }
        }

        public int GetTotalAdminsCount()
        {
            using (var _context = new DatabaseContext())
            {
                string RoleCode = "Manager";
                var data = (from user in _context.Registration
                            join role in _context.Role on user.RoleID equals role.RoleID
                            where role.RoleCode == RoleCode
                            select user).Count();
                return data;
            }
        }

        public int GetTotalUsersCount()
        {
            using (var _context = new DatabaseContext())
            {

                string[] RoleCodes = new string[] { "SuperAdmin", "Manager" };
                var data = (from user in _context.Registration
                            join role in _context.Role on user.RoleID equals role.RoleID
                            where !RoleCodes.Contains(role.RoleCode)
                            select user).Count();
                return data;
            }
        }

        public RegistrationViewDetailsModel GetUserDetailsByRegistrationID(int? RegistrationID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    //string[] RoleCodes = new string[] { "SuperAdmin", "Manager" };
                    var queryResult = (from registration in _context.Registration
                                       join A in _context.AssignedRoles on registration.RegistrationID equals A.RegistrationID into A_join
                                       from A in A_join.DefaultIfEmpty()
                                       join AA in _context.Registration on A.AssignToAdmin equals AA.RegistrationID into AA_join
                                       from AA in AA_join.DefaultIfEmpty()
                                       join role in _context.Role on registration.RoleID equals role.RoleID into RL_join
                                       from role in RL_join.DefaultIfEmpty()
                                       join department in _context.Departments on registration.DeptId equals department.DeptId into D_join
                                       from department in D_join.DefaultIfEmpty()
                                       join job in _context.Jobs on registration.JobId equals job.JobId into J_join
                                       from job in J_join.DefaultIfEmpty()
                                       where registration.RegistrationID == RegistrationID
                                       select new RegistrationViewModel()
                                       {
                                           registration = registration,
                                           roles = role,
                                           departments = department,
                                           jobs = job,
                                           ManagerFirstName = AA.FirstName,
                                           ManagerLastName = AA.LastName,
                                           ManagerMiddleName = AA.MiddleName
                                       }).FirstOrDefault();
                    if (queryResult != null)
                    {
                        var objRegistrationViewDetailsModel = new RegistrationViewDetailsModel()
                        {
                            Birthdate = string.IsNullOrEmpty(queryResult.registration.Birthdate.ToString()) ? "" : Convert.ToDateTime(queryResult.registration.Birthdate, new CultureInfo("es-US", true)).ToString("MM/dd/yyyy"),
                            DateofJoining = string.IsNullOrEmpty(queryResult.registration.DateofJoining.ToString()) ? "" : Convert.ToDateTime(queryResult.registration.DateofJoining, new CultureInfo("es-US", true)).ToString("MM/dd/yyyy"),
                            DateofLeaving = string.IsNullOrEmpty(queryResult.registration.DateofLeaving.ToString()) ? "" : Convert.ToDateTime(queryResult.registration.DateofLeaving, new CultureInfo("es-US", true)).ToString("MM/dd/yyyy"),
                            EmailID = queryResult.registration.EmailID,
                            EmployeeID = queryResult.registration.EmployeeID,
                            Gender = queryResult.registration.Gender,
                            IsActive = Convert.ToBoolean(queryResult.registration.IsActive),
                            Mobileno = queryResult.registration.Mobileno,
                            FirstName = queryResult.registration.FirstName,
                            MiddleName = queryResult.registration.MiddleName,
                            LastName = queryResult.registration.LastName,
                            RegistrationID = queryResult.registration.RegistrationID,
                            RoleID = Convert.ToInt32(queryResult.registration.RoleID),
                            Username = queryResult.registration.Username,
                            WorkEmail = queryResult.registration.WorkEmail,
                            EmergencyContact = queryResult.registration.EmergencyContact,
                            EmergencyContactNumber = queryResult.registration.EmergencyContactNumber,
                            DeptId = Convert.ToInt32(queryResult.registration.DeptId),
                            ManagerId = Convert.ToInt32(queryResult.registration.ManagerId),
                            JobId = Convert.ToInt32(queryResult.registration.JobId),
                            JobTitle = queryResult.jobs == null ? "" : queryResult.jobs.JobTitle,
                            RoleName = queryResult.roles == null ? "" : queryResult.roles.Rolename,
                            DepartmentName = queryResult.departments == null ? "" : queryResult.departments.DepartmentName,
                            ManagerFirstName = queryResult.ManagerFirstName == null ? "" : queryResult.ManagerFirstName,
                            ManagerMiddleName = queryResult.ManagerMiddleName == null ? "" : queryResult.ManagerMiddleName,
                            ManagerLastName = queryResult.ManagerLastName == null ? "" : queryResult.ManagerLastName
                        };
                        return objRegistrationViewDetailsModel;
                    }
                    return new RegistrationViewDetailsModel();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetUserIDbyExpenseID(int ExpenseID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from tm in _context.ExpenseModel
                            where tm.ExpenseID == ExpenseID
                            select tm.UserID).Take(1).FirstOrDefault();
                return data;
            }
        }

        public int GetUserIDbyTimesheetID(int TimeSheetMasterID)
        {

            using (var _context = new DatabaseContext())
            {
                var data = (from tm in _context.TimeSheetMaster
                            where tm.TimeSheetMasterID == TimeSheetMasterID
                            select tm.UserID).Take(1).FirstOrDefault();
                return data;
            }

        }

        public List<RegistrationViewSummaryModel> ShowallAdmin(string sortColumn, string sortColumnDir, string Search)
        {

            using (var _context = new DatabaseContext())
            {
                string RoleCode = "Manager";
                var IQueryabletimesheet = (from registration in _context.Registration
                                           join role in _context.Role on registration.RoleID equals role.RoleID
                                           where role.RoleCode == RoleCode
                                           select new RegistrationViewSummaryModel
                                           {
                                               FirstName = registration.FirstName,
                                               MiddleName = registration.MiddleName,
                                               LastName = registration.LastName,
                                               RegistrationID = registration.RegistrationID,
                                               WorkEmail = registration.EmailID,
                                               Mobileno = registration.Mobileno,
                                               Username = registration.Username,
                                               IsActive = registration.IsActive,
                                               EmployeeID = registration.EmployeeID
                                           });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName.Contains(Search) || m.MiddleName.Contains(Search) || m.LastName.Contains(Search));
                }

                return IQueryabletimesheet.ToList();
            }
        }

        public IQueryable<RegistrationViewSummaryModel> ShowallUsers(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();
            var IQueryabletimesheet = (from registration in _context.Registration
                                       join AssignedRoles in _context.AssignedRoles
                                     on registration.RegistrationID equals AssignedRoles.RegistrationID into roleGroup
                                       from m in roleGroup.DefaultIfEmpty()
                                       join AssignedRolesAdmin in _context.Registration
                                            on m.AssignToAdmin equals AssignedRolesAdmin.RegistrationID into AssignedRolesAdminGroup
                                       from p in AssignedRolesAdminGroup.DefaultIfEmpty()
                                       orderby registration.LastName descending
                                       select new RegistrationViewSummaryModel
                                       {
                                           FirstName = registration.FirstName,
                                           MiddleName = registration.MiddleName,
                                           LastName = registration.LastName,
                                           AssignToAdmin = string.IsNullOrEmpty(p.FirstName) ? "*Not Assigned*" : (p.FirstName + " " + p.MiddleName + " " + p.LastName),
                                           RegistrationID = registration.RegistrationID,
                                           WorkEmail = registration.WorkEmail,
                                           Mobileno = registration.Mobileno,
                                           Username = registration.Username,
                                           RoleId = registration.RoleID,
                                           IsActive = registration.IsActive,
                                           EmployeeID = registration.EmployeeID,
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName.Contains(Search) || m.MiddleName.Contains(Search) || m.LastName.Contains(Search));
            }
            return IQueryabletimesheet;
        }
        public IQueryable<RegistrationViewSummaryModel> ShowallUsersUnderAdmin(string sortColumn, string sortColumnDir, string Search, int? RegistrationID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from registration in _context.Registration
                                       join AssignedRoles in _context.AssignedRoles on registration.RegistrationID equals AssignedRoles.RegistrationID
                                       where AssignedRoles.AssignToAdmin == RegistrationID
                                       select new RegistrationViewSummaryModel
                                       {
                                           FirstName = registration.FirstName,
                                           MiddleName = registration.MiddleName,
                                           LastName = registration.LastName,
                                           RegistrationID = registration.RegistrationID,
                                           WorkEmail = registration.WorkEmail,
                                           Mobileno = registration.Mobileno,
                                           Username = registration.Username,
                                           IsActive = registration.IsActive,
                                           EmployeeID = registration.EmployeeID
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName.Contains(Search) || m.MiddleName.Contains(Search) || m.LastName.Contains(Search) || m.Username.Contains(Search) || m.Mobileno.Contains(Search) || m.EmployeeID.Contains(Search) || m.Mobileno.Contains(Search));
            }

            return IQueryabletimesheet;

        }
       
    }
}
