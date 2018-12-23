using System;
using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
using POC.ViewModels;
using System.Data.Entity;

namespace POC.Repository.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        public int Save(TaskViewModel taskModelName)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var Task = (from t in _context.Tasks.Where(x => x.TaskID == taskModelName.TaskID)
                                select t).FirstOrDefault();
                    if (Task != null)
                    {
                        Task.Status = taskModelName.Status;
                        Task.Comments = taskModelName.Comments;
                        _context.Entry(Task).State = EntityState.Modified;
                        return _context.SaveChanges();
                    }
                    else
                    {
                        var task = new TaskTB()
                        {
                            ProjectID = taskModelName.ProjectID,
                            Taskname = taskModelName.Taskname,
                            IsActive = taskModelName.IsActive,
                            Status = taskModelName.Status,
                            AssignedtoID = taskModelName.AssignedtoID,
                            Comments = taskModelName.Comments
                        };
                        _context.Tasks.Add(task);
                        return _context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsTaskExists(int ProjectId, string TaskName)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from task in _context.Tasks
                                  where task.ProjectID == ProjectId && task.Taskname == TaskName
                                  select task).Count();
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
        public List<TaskViewModel> GetTasksByProjectId(int ProjectId)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var listofTasks = (from task in _context.Tasks.Where(x => x.ProjectID == ProjectId)
                                       join project in _context.ProjectMaster on task.ProjectID equals project.ProjectID
                                       select new TaskViewModel()
                                       {
                                           Taskname = task.Taskname,
                                           TaskID = task.TaskID,
                                           IsActive = task.IsActive.HasValue ? task.IsActive : true,
                                           IsDeleted = task.IsDeleted.HasValue ? task.IsDeleted : true,
                                           ProjectName = project.ProjectName
                                       }).ToList();
                    return listofTasks;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TaskViewModel> GetTasksByUserId(int ProjectId, int UserId)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    if (ProjectId > 0 && UserId >0)
                    {
                        var listofTasks = (from task in _context.Tasks.Where(x => x.ProjectID == ProjectId && x.AssignedtoID == UserId)
                                           join project in _context.ProjectMaster on task.ProjectID equals project.ProjectID
                                           join user in _context.Registration on task.AssignedtoID equals user.RegistrationID into rj
                                           from userset in rj.DefaultIfEmpty()
                                           select new TaskViewModel()
                                           {
                                               Taskname = task.Taskname,
                                               TaskID = task.TaskID,
                                               IsActive = task.IsActive.HasValue ? task.IsActive : true,
                                               IsDeleted = task.IsDeleted.HasValue ? task.IsDeleted : true,
                                               ProjectName = project.ProjectName,
                                               FirstName= userset.FirstName,
                                               MiddleName=userset.MiddleName,
                                               LastName=userset.LastName,
                                               StatusType = task.Status == 1 ? "New" : task.Status == 2? "Active" : task.Status == 3 ? "Completed" : "UnKnown",
                                           }).ToList();

                        return listofTasks;
                    }
                    else if (ProjectId > 0 && UserId == 0)
                    {
                        var listofTasks = (from task in _context.Tasks.Where(x => x.ProjectID == ProjectId)
                                           join project in _context.ProjectMaster on task.ProjectID equals project.ProjectID
                                           join user in _context.Registration on task.AssignedtoID equals user.RegistrationID into rj
                                           from userset in rj.DefaultIfEmpty()
                                           select new TaskViewModel()
                                           {
                                               Taskname = task.Taskname,
                                               TaskID = task.TaskID,
                                               IsActive = task.IsActive.HasValue ? task.IsActive : true,
                                               IsDeleted = task.IsDeleted.HasValue ? task.IsDeleted : true,
                                               ProjectName = project.ProjectName,
                                               FirstName = userset.FirstName,
                                               MiddleName = userset.MiddleName,
                                               LastName = userset.LastName,
                                               StatusType = task.Status == 1 ? "New" : task.Status == 2 ? "Active" : task.Status == 3 ? "Completed" : "UnKnown",
                                           }).ToList();

                        return listofTasks;
                    }
                    else if (ProjectId == 0 && UserId > 0)
                    {
                        var listofTasks = (from task in _context.Tasks.Where(x =>x.AssignedtoID == UserId)
                                           join project in _context.ProjectMaster on task.ProjectID equals project.ProjectID
                                           join user in _context.Registration on task.AssignedtoID equals user.RegistrationID into rj
                                           from userset in rj.DefaultIfEmpty()
                                           select new TaskViewModel()
                                           {
                                               Taskname = task.Taskname,
                                               TaskID = task.TaskID,
                                               IsActive = task.IsActive.HasValue ? task.IsActive : true,
                                               IsDeleted = task.IsDeleted.HasValue ? task.IsDeleted : true,
                                               ProjectName = project.ProjectName,
                                               FirstName = userset.FirstName,
                                               MiddleName = userset.MiddleName,
                                               LastName = userset.LastName,
                                               StatusType = task.Status == 1 ? "New" : task.Status == 2 ? "Active" : task.Status == 3 ? "Completed" : "UnKnown",
                                           }).ToList();

                        return listofTasks;
                    }
                    else if (ProjectId == 0 && UserId == 0)
                    {
                        var listofTasks = (from task in _context.Tasks
                                           join project in _context.ProjectMaster on task.ProjectID equals project.ProjectID
                                           join user in _context.Registration on task.AssignedtoID equals user.RegistrationID into rj
                                           from userset in rj.DefaultIfEmpty()
                                           select new TaskViewModel()
                                           {
                                               Taskname = task.Taskname,
                                               TaskID = task.TaskID,
                                               IsActive = task.IsActive.HasValue ? task.IsActive : true,
                                               IsDeleted = task.IsDeleted.HasValue ? task.IsDeleted : true,
                                               ProjectName = project.ProjectName,
                                               FirstName = userset.FirstName,
                                               MiddleName = userset.MiddleName,
                                               LastName = userset.LastName,
                                               StatusType = task.Status == 1 ? "New" : task.Status == 2 ? "Active" : task.Status == 3 ? "Completed" : "UnKnown",
                                           }).ToList();

                        return listofTasks;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TaskViewModel GetById(int ID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var task = (from p in _context.Tasks.Where(x => x.TaskID == ID)
                                select new TaskViewModel()
                                {
                                    TaskID = p.TaskID,
                                    Taskname = p.Taskname,
                                    ProjectID = p.ProjectID,
                                    IsActive = p.IsActive,
                                    IsDeleted = p.IsDeleted,
                                    Status = p.Status,
                                    Comments = p.Comments
                                }).FirstOrDefault();
                    return task;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
