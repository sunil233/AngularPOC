using System;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
using System.Linq.Dynamic;
using System.Data.Entity;
using POC.ViewModels;
using System.Globalization;


namespace POC.Repository.Implementation
{
    public class RegistrationRepository : IRegistrationRepository
    {
        public bool CheckUserNameExists(string Username)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from user in _context.Registration
                                  where user.Username == Username
                                  select user).Count();

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
        public int AddUser(Registration entity)
        {
            var result = 0;
            try
            {
                using (var _context = new DatabaseContext())
                {
                    entity.Birthdate = Convert.ToDateTime(entity.Birthdate, new CultureInfo("es-US", true));
                    entity.DateofJoining = Convert.ToDateTime(entity.DateofJoining, new CultureInfo("es-US", true));
                    entity.DateofLeaving = Convert.ToDateTime(entity.DateofLeaving, new CultureInfo("es-US", true));
                    _context.Registration.Add(entity);
                    if (entity.RegistrationID > 0)
                    {
                        UpdateEmpId(entity.RegistrationID);
                    }
                    result = _context.SaveChanges();

                    return entity.RegistrationID;
                }
            }          
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int UpdateEmpId(int RegId)
        {
            int result = 0;
            using (var _context = new DatabaseContext())
            {
                var user = (from register in _context.Registration.Where(x => x.RegistrationID == RegId)
                            select register).FirstOrDefault();
                user.EmployeeID = "TM" + Convert.ToString(RegId).PadLeft(3, '0');
                _context.Entry(user).State = EntityState.Modified;
                result = _context.SaveChanges();
            }
            return result;
        }
        public int UpdateUser(Registration entity)
        {
            var result = 0;
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var user = (from register in _context.Registration.Where(x => x.RegistrationID == entity.RegistrationID)
                                select register).FirstOrDefault();
                    if (user != null)
                    {
                        user.RegistrationID = entity.RegistrationID;
                        user.Birthdate = Convert.ToDateTime(entity.Birthdate, new CultureInfo("es-US", true));
                        user.Gender = entity.Gender;
                        user.Mobileno = entity.Mobileno;
                        user.FirstName = entity.FirstName;
                        user.MiddleName = entity.MiddleName;
                        user.LastName = entity.LastName;
                        if (user.DateofLeaving == null)
                        {
                            user.IsActive = true;
                        }
                        else
                        {
                            user.IsActive = false;
                        }
                        //work info
                        user.RoleID = entity.RoleID;
                        user.DateofJoining = Convert.ToDateTime(entity.DateofJoining, new CultureInfo("es-US", true));
                        user.DateofLeaving = Convert.ToDateTime(entity.DateofLeaving, new CultureInfo("es-US", true));
                        user.EmailID = entity.EmailID;
                        user.ManagerId = entity.ManagerId;
                        user.JobId = entity.JobId;
                        user.DeptId = entity.DeptId;
                        user.EmergencyContact = entity.EmergencyContact;
                        user.EmergencyContactNumber = entity.EmergencyContactNumber;
                        user.WorkEmail = entity.WorkEmail;
                        user.EmployeeID = entity.EmployeeID;
                        _context.Entry(user).State = EntityState.Modified;
                        result = _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdatePassword(int RegistrationID, string Password)
        {
            try
            {
                var result = 0;
                using (var _context = new DatabaseContext())
                {
                    var user = (from register in _context.Registration.Where(x => x.RegistrationID == RegistrationID)
                                select register).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Password;
                        user.ConfirmPassword = Password;
                        _context.Entry(user).State = EntityState.Modified;
                        result = _context.SaveChanges();
                        return true; ;
                    }
                    else
                    {
                        return false;
                    }
                }


            }
            catch (Exception)
            {
                return false;
            }
        }
        public int CanDeleteUser(int RegistrationID)
        {

            using (var _context = new DatabaseContext())
            {
                var IQueryableCount = (from register in _context.AssignedRoles.Where(x => x.AssignToAdmin == RegistrationID)
                                       select register).Count();
                return IQueryableCount;
            }

        }
        public int DeleteUser(int RegistrationID)
        {
            var result = 0;
            using (var _context = new DatabaseContext())
            {
                var entity = (from register in _context.Registration.Where(x => x.RegistrationID == RegistrationID)
                              select register).FirstOrDefault();
                if (entity != null)
                {
                    _context.Registration.Remove(entity);
                    result = _context.SaveChanges();
                }
                return result;
            }
        }
        public IQueryable<RegistrationViewSummaryModel> ListofRegisteredUser(string sortColumn, string sortColumnDir, string Search)
        {
            try
            {
                var _context = new DatabaseContext();

                var IQueryableRegistered = (from registration in _context.Registration
                                            select new RegistrationViewSummaryModel
                                            {
                                                FirstName = registration.FirstName,
                                                MiddleName = registration.MiddleName,
                                                LastName = registration.LastName,
                                                RegistrationID = registration.RegistrationID,
                                                WorkEmail = registration.WorkEmail,
                                                Mobileno = registration.Mobileno,
                                                Username = registration.Username,
                                                RoleId = registration.RoleID,
                                                IsActive = registration.IsActive
                                            }
                                );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    IQueryableRegistered = IQueryableRegistered.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    IQueryableRegistered = IQueryableRegistered.Where(m => m.Username.Contains(Search) || m.FirstName.Contains(Search) || m.MiddleName.Contains(Search) || m.LastName.Contains(Search));
                }

                return IQueryableRegistered;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
