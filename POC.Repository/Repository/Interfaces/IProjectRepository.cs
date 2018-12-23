
using System.Collections.Generic;
using System.Linq;
using POC.Models;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IProjectRepository
    {
       
        int Save(ProjectMaster ProjectMaster);
        ProjectMasterViewModel GetById(int ID);
        IQueryable<ProjectMasterViewModel> GetAll(string sortColumn, string sortColumnDir, string Search);
        IQueryable<ProjectMasterViewModel> GetAllByRoleCode(string sortColumn, string sortColumnDir, string Search, string RoleCode);
        List<ProjectMasterViewModel> GetListofProjects();
        int Delete(int ProjectID);
        bool CheckProjectCodeExists(string ProjectCode);
        bool CheckProjectNameExists(string ProjectName);
        bool CheckProjectIDExistsInTimesheet(int ProjectID);
        bool CheckProjectIDExistsInExpense(int ProjectID);       
        int GetTotalProjectsCounts();
        bool SaveAssignedProjects(AssignProjects assignProjectsModel);
        List<ProjectMasterViewModel> GetUnAssignedProjects();
        List<ProjectMasterViewModel> GetAdminProjects(int AdminId);
        List<ProjectMasterViewModel> GetAssignedProjects(int userId);
        bool RemoveProject(int userId, int projectId);
    }

}
