using System;
using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
using System.Data.Entity.SqlServer;
using System.Linq.Dynamic;
using POC.ViewModels;
using System.Data.Entity;

namespace POC.Repository.Implementation
{
    public class AssignRolesRepository : IAssignRolesRepository
    {
        public List<AdminModel> ListofAdmins()
        {

            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from register in _context.Registration
                                  join role in _context.Role on register.RoleID equals role.RoleID
                                  where role.RoleCode == "Manager"
                                  select new AdminModel()
                                  {
                                      FirstName = register.FirstName,
                                      LastName = register.LastName,
                                      MiddleName = register.MiddleName,
                                      RegistrationID = SqlFunctions.StringConvert((double?)register.RegistrationID).Trim()
                                  }).ToList();
                    if (result != null)
                    {
                        result.Insert(0, new AdminModel { FullName = "----Select----", RegistrationID = "" });
                        return result;
                    }
                    else
                    {
                        return new List<AdminModel>();
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<UserModel> ListofUser()
        {

            using (var _context = new DatabaseContext())
            {

                string[] RoleCodes = new string[] { "SuperAdmin", "Manager" };
                var result = (from register in _context.Registration
                              join role in _context.Role on register.RoleID equals role.RoleID
                              where !RoleCodes.Contains(role.RoleCode)
                              select new UserModel()
                              {
                                  FirstName = register.FirstName,
                                  MiddleName = register.MiddleName,
                                  LastName = register.LastName,
                                  RegistrationID = register.RegistrationID

                              }).ToList();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<UserModel>();
                }
            }

        }
        public IQueryable<UserModel> ShowallRoles(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from AssignedRoles in _context.AssignedRoles
                                       join registration in _context.Registration on AssignedRoles.RegistrationID equals registration.RegistrationID
                                       join p in _context.Registration on AssignedRoles.AssignToAdmin equals p.RegistrationID
                                       select new UserModel
                                       {
                                           FirstName = registration.FirstName,
                                           MiddleName = registration.MiddleName,
                                           LastName = registration.LastName,
                                           AdminFirstName = p.FirstName,
                                           AdminMiddleName = p.MiddleName,
                                           AdminLastName = p.LastName,
                                           RegistrationID = registration.RegistrationID
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName.Contains(Search));
            }

            return IQueryabletimesheet;

        }

        public bool RemovefromUserRole(int RegistrationID)
        {
            var result = 0;
            using (var _context = new DatabaseContext())
            {
                var entity = (from register in _context.AssignedRoles.Where(x => x.RegistrationID == RegistrationID)
                              select register).FirstOrDefault();
                if (entity != null)
                {
                    _context.AssignedRoles.Remove(entity);
                    result = _context.SaveChanges();
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
            return true;
        }

        public List<UserModel> GetListofUnAssignedUsers()
        {
            try
            {
                var db = new DatabaseContext();
                string[] RoleCodes = new string[] { "SuperAdmin", "Manager" };
                var listusers = (from R in db.Registration
                                 join RL in db.Role on R.RoleID equals RL.RoleID
                                 where !RoleCodes.Contains(RL.RoleCode) &&
                                   !(from AssignedRoles in db.AssignedRoles
                                     select new
                                     {
                                         AssignedRoles.RegistrationID
                                     }).ToList().Contains(new { RegistrationID = R.RegistrationID })
                                 select new UserModel()
                                 {
                                     RegistrationID = R.RegistrationID,
                                     FirstName = R.FirstName,
                                     MiddleName = R.MiddleName,
                                     LastName = R.LastName
                                 }).ToList();
                return listusers;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public bool SaveAssignedRoles(AssignRolesModel AssignRolesModel)
        {
            bool result = false;
            using (var _context = new DatabaseContext())
            {
                try
                {
                    for (int i = 0; i < AssignRolesModel.ListofUser.Count(); i++)
                    {
                        if (AssignRolesModel.ListofUser[i].selectedUsers)
                        {
                            AssignedRoles AssignedRoles = new AssignedRoles
                            {
                                AssignedRolesID = 0,
                                AssignToAdmin = AssignRolesModel.RegistrationID,
                                CreatedOn = DateTime.Now,
                                CreatedBy = AssignRolesModel.CreatedBy,
                                Status = "A",
                                RegistrationID = AssignRolesModel.ListofUser[i].RegistrationID
                            };

                            _context.AssignedRoles.Add(AssignedRoles);
                            _context.SaveChanges();
                        }
                    }

                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }

                return result;
            }
        }


        public bool AssignManager(AssignRolesModel AssignRolesModel)
        {
            bool result = false;
            using (var _context = new DatabaseContext())
            {
                try
                {
                    var role = _context.AssignedRoles.Where(x => x.RegistrationID == AssignRolesModel.RegistrationID).FirstOrDefault();
                    if (role != null)
                    {
                        role.AssignToAdmin = AssignRolesModel.AssignToAdmin;
                        role.RegistrationID = AssignRolesModel.RegistrationID;
                        role.Status = "A";
                        _context.Entry(role).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        AssignedRoles AssignedRoles = new AssignedRoles
                        {
                            AssignToAdmin = AssignRolesModel.AssignToAdmin,
                            RegistrationID = AssignRolesModel.RegistrationID,
                            Status = "A",
                            CreatedOn = DateTime.Now,
                            CreatedBy = AssignRolesModel.CreatedBy,
                            AssignedRolesID = 0,
                        };
                        _context.AssignedRoles.Add(AssignedRoles);
                        _context.SaveChanges();
                    }
                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }
                return result;
            }
        }
        public bool CheckIsUserAssignedRole(int RegistrationID)
        {
            var IsassignCount = 0;
            using (var _context = new DatabaseContext())
            {
                IsassignCount = (from assignUser in _context.AssignedRoles
                                 where assignUser.RegistrationID == RegistrationID
                                 select assignUser).Count();
            }

            if (IsassignCount > 0)
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
