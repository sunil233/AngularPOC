using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using POC.Repository.Interface;
using POC.Models;
namespace POC.Repository.Implementation
{
    public class RoleRepository : IRoleRepository
    {

        /// <summary>
        /// Get RoleID Name by RoleName
        /// </summary>
        /// <param name="Rolename"></param>
        /// <returns></returns>
        public int getRolesofUserbyRolename(string Rolename)
        {
            using (var _context = new DatabaseContext())
            {
                var roleID = (from role in _context.Role
                              where role.Rolename == Rolename
                              select role.RoleID).SingleOrDefault();

                return roleID;
            }
        }
        public List<Roles> getRoles()
        {
            using (var _context = new DatabaseContext())
            {
                var roles = (from role in _context.Role
                             select role).ToList();
                return roles;
            }
        }
        public int AddRole(Roles role)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    role.IsActive = true;
                    _context.Role.Add(role);
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int UpdateRole(Roles role)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    role.IsActive = true;
                    _context.Entry(role).State = EntityState.Modified;
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int DeleteRole(int ID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var role = (from r in _context.Role
                                where r.RoleID == ID
                                select r).FirstOrDefault();
                    if (role != null)
                    {
                        _context.Role.Remove(role);
                        return _context.SaveChanges();
                    }
                    return -1;
                   
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.ToString().Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new Exception("This Role is associated with Employee.Please Unlink the Employee(s) with this Role.");
                }
                throw new Exception(ex.Message);
            }
        }
        public bool IsRoleExist(string RoleName)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from role in _context.Role
                                  where role.Rolename == RoleName
                                  select role).Count();

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
        public string GetRoleName(int? RoleId)
        {
            using (var _context = new DatabaseContext())
            {
                var objrole = (from role in _context.Role
                               where role.RoleID == RoleId
                               select role).FirstOrDefault();
                if (objrole != null)
                    return objrole.Rolename;
                else
                    return "";
            }
        }

        public string GetRoleCode(int? RoleId)
        {
            using (var _context = new DatabaseContext())
            {
                var objrole = (from role in _context.Role
                               where role.RoleID == RoleId
                               select role).FirstOrDefault();
                if (objrole != null)
                    return objrole.RoleCode;
                else
                    return "";
            }
        }

        public Roles GetById(int ID)
        {
            using (var _context = new DatabaseContext())
            {
                var objrole = (from role in _context.Role
                               where role.RoleID == ID
                               select role).FirstOrDefault();
                return objrole;
            }
        }

        public bool CheckRoleCodeExists(string RoleCode)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from role in _context.Role
                                  where role.RoleCode == RoleCode
                                  select role).Count();

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
        public bool CheckRoleNameExists(string RoleName)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from role in _context.Role
                                  where role.Rolename == RoleName
                                  select role).Count();

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
    }
}
