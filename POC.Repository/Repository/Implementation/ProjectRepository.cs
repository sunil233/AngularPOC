using System;
using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
using System.Linq.Dynamic;
using POC.ViewModels;


namespace POC.Repository.Implementation
{
    public class ProjectRepository : IProjectRepository
    {
        public bool CheckProjectCodeExists(string ProjectCode)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from user in _context.ProjectMaster
                                  where user.ProjectCode == ProjectCode
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
        public bool CheckProjectNameExists(string ProjectName)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from user in _context.ProjectMaster
                                  where user.ProjectName == ProjectName
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
        public ProjectMasterViewModel GetById(int ID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var project = (from p in _context.ProjectMaster
                                   where p.ProjectID == ID
                                   select new ProjectMasterViewModel()
                                   {
                                       ProjectID = p.ProjectID,
                                       ProjectCode = p.ProjectCode,
                                       ProjectName = p.ProjectName,
                                       IsActive = p.IsActive
                                   }).FirstOrDefault();
                    return project;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ProjectMasterViewModel> GetListofProjects()
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var listofProjects = (from projectmaster in _context.ProjectMaster
                                          where projectmaster.IsDeleted == false
                                          select new ProjectMasterViewModel
                                          {
                                              ProjectID = projectmaster.ProjectID,
                                              ProjectCode = projectmaster.ProjectCode,
                                              ProjectName = projectmaster.ProjectName,
                                              IsActive = projectmaster.IsActive,
                                              Status = projectmaster.IsActive ? "Active" : "InActive"
                                          }).ToList();
                    return listofProjects;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Save(ProjectMaster ProjectMaster)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.ProjectMaster.Add(ProjectMaster);
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<ProjectMasterViewModel> GetAll(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();
            var IQueryableproject = (from projectmaster in _context.ProjectMaster
                                    
                                     select new ProjectMasterViewModel
                                     {
                                         ProjectID = projectmaster.ProjectID,
                                         ProjectCode = projectmaster.ProjectCode,
                                         ProjectName = projectmaster.ProjectName,
                                         IsActive = projectmaster.IsActive,
                                         Status = projectmaster.IsActive ? "Active" : "InActive"                                       

                                     });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableproject = IQueryableproject.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableproject = IQueryableproject.Where(m => m.ProjectName == Search || m.ProjectCode == Search);
            }
            return IQueryableproject;
        }
        public IQueryable<ProjectMasterViewModel> GetAllByRoleCode(string sortColumn, string sortColumnDir, string Search, string RoleCode)
        {
            var _context = new DatabaseContext();
            var IQueryableproject = (from projectmaster in _context.ProjectMaster
                                     join _assignedProjects in _context.AssignedProjects on projectmaster.ProjectID equals _assignedProjects.ProjectId into gj
                                     from projectset in gj.DefaultIfEmpty()
                                     join user in _context.Registration on projectset.ManagerId equals user.RegistrationID into rj
                                     from userset in rj.DefaultIfEmpty()
                                     join role in _context.Role on userset.RoleID equals role.RoleID into rs
                                     from roleset in rs.DefaultIfEmpty()
                                     where roleset.RoleCode == RoleCode
                                     select new ProjectMasterViewModel
                                     {
                                         ProjectID = projectmaster.ProjectID,
                                         ProjectCode = projectmaster.ProjectCode,
                                         ProjectName = projectmaster.ProjectName,
                                         IsActive = projectmaster.IsActive,
                                         Status = projectmaster.IsActive ? "Active" : "InActive",
                                         FirstName = userset.FirstName,
                                         MiddleName = userset.MiddleName,
                                         LastName = userset.LastName,
                                         RoleName = roleset.Rolename,
                                         ManagerId = userset.RegistrationID

                                     });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableproject = IQueryableproject.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableproject = IQueryableproject.Where(m => m.ProjectName == Search || m.ProjectCode == Search);
            }
            return IQueryableproject;
        }
        public bool CheckProjectIDExistsInTimesheet(int ProjectID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from timesheet in _context.TimeSheetDetails
                                  where timesheet.ProjectID == ProjectID
                                  select timesheet).Count();

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
        public bool CheckProjectIDExistsInExpense(int ProjectID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from expense in _context.ExpenseModel
                                  where expense.ProjectID == ProjectID
                                  select expense).Count();

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
        public int Delete(int ProjectID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var project = (from expense in _context.ProjectMaster
                                   where expense.ProjectID == ProjectID
                                   select expense).SingleOrDefault(); ;
                    if (project != null)
                    {
                        _context.ProjectMaster.Remove(project);
                        int resultProject = _context.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.ToString().Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new Exception("This Project is associated with Tasks.Please Unlink the Tasks(s)associated with this Project.");
                }
                throw new Exception(ex.Message);
            }
        }
        public int GetTotalProjectsCounts()
        {
            using (var _context = new DatabaseContext())
            {
                var projectsCount = (from project in _context.ProjectMaster
                                     select project).Count();
                return projectsCount;
            }

        }
        public bool SaveAssignedProjects(AssignProjects assignProjectsModel)
        {
            bool result = false;
            using (var _context = new DatabaseContext())
            {
                try
                {
                    if (assignProjectsModel.Projects != null)
                    {
                        for (int i = 0; i < assignProjectsModel.Projects.Count(); i++)
                        {
                            var AssignedProjects = new AssignedProjects
                            {
                                ManagerId = assignProjectsModel.ManagerId,
                                Status = "A",
                                ProjectId = assignProjectsModel.Projects[i].ProjectID
                            };
                            _context.AssignedProjects.Add(AssignedProjects);
                            _context.SaveChanges();
                        }
                    }
                    else if (assignProjectsModel.Users != null)
                    {
                        for (int i = 0; i < assignProjectsModel.Users.Count(); i++)
                        {
                            var AssignedProjects = new AssignedProjects
                            {
                                ManagerId = assignProjectsModel.Users[i].RegistrationID,
                                Status = "A",
                                ProjectId = assignProjectsModel.ProjectId
                            };
                            _context.AssignedProjects.Add(AssignedProjects);
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
        public List<ProjectMasterViewModel> GetUnAssignedProjects()
        {
            try
            {
                var db = new DatabaseContext();
                var projectslist = (from project in db.ProjectMaster
                                    where !db.AssignedProjects.Any(f => f.ProjectId == project.ProjectID)
                                    select new ProjectMasterViewModel()
                                    {
                                        ProjectID = project.ProjectID,
                                        ProjectCode = project.ProjectCode,
                                        ProjectName = project.ProjectName
                                    }).ToList();
                return projectslist;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectMasterViewModel> GetAdminProjects(int AdminId)
        {
            var _context = new DatabaseContext();
            var IQueryableproject = (from projectmaster in _context.ProjectMaster
                                     join _assignedProjects in _context.AssignedProjects on projectmaster.ProjectID equals _assignedProjects.ProjectId into gj
                                     from projectset in gj.DefaultIfEmpty()
                                     join user in _context.Registration on projectset.ManagerId equals user.RegistrationID into rj
                                     from userset in rj.DefaultIfEmpty()
                                     where userset.RegistrationID == AdminId
                                     select new ProjectMasterViewModel
                                     {
                                         ProjectID = projectmaster.ProjectID,
                                         ProjectCode = projectmaster.ProjectCode,
                                         ProjectName = projectmaster.ProjectName,
                                         IsActive = projectmaster.IsActive,
                                         Status = projectmaster.IsActive ? "Active" : "InActive",
                                         FirstName = userset.FirstName,
                                         MiddleName = userset.MiddleName,
                                         LastName = userset.LastName
                                     }).ToList();

            return IQueryableproject;
        }
        public List<ProjectMasterViewModel> GetAssignedProjects(int userId)
        {

            var _context = new DatabaseContext();
            var queryResult = (from registration in _context.Registration
                               join A in _context.AssignedRoles on registration.RegistrationID equals A.RegistrationID into A_join
                               from A in A_join.DefaultIfEmpty()
                               join AA in _context.Registration on A.AssignToAdmin equals AA.RegistrationID into AA_join
                               from AA in AA_join.DefaultIfEmpty()
                               join _assignedProjects in _context.AssignedProjects on registration.RegistrationID equals _assignedProjects.ManagerId into RL_join
                               from _assignedProjects in RL_join.DefaultIfEmpty()
                               join projectmaster in _context.ProjectMaster on _assignedProjects.ProjectId equals projectmaster.ProjectID into D_join
                               from Proj in D_join.DefaultIfEmpty()
                               where registration.RegistrationID == userId
                               select new ProjectMasterViewModel()
                               {
                                   ProjectID = Proj.ProjectID,
                                   ProjectCode= Proj.ProjectCode,
                                   ProjectName= Proj.ProjectName,
                                   FirstName = AA.FirstName,
                                   LastName = AA.LastName,
                                   MiddleName = AA.MiddleName,
                                   IsActive=Proj.IsActive
                               }).ToList();
            return queryResult;
        }

        public bool RemoveProject(int userId, int projectId)
        {
            var result = 0;
            using (var _context = new DatabaseContext())
            {
                var entity = (from register in _context.AssignedProjects.Where(x => x.ManagerId == userId && x.ProjectId== projectId)
                              select register).FirstOrDefault();
                if (entity != null)
                {
                    _context.AssignedProjects.Remove(entity);
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
    }
}
