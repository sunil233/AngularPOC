using POC.Models;
using POC.Repository.Interface;
using POC.ViewModels;
using System;
using System.Linq;
using System.Web.Http;
namespace POC.Controllers
{

    public class ProjectController : ApiController
    {
      
        private readonly IProjectRepository _IProjectRepository;
        private readonly ILogger _logger;
        public ProjectController(IProjectRepository IProjectRepository,ILogger logger)
        {         
            _IProjectRepository = IProjectRepository;
            _logger = logger;
        }

        /// <summary>
        /// Add Project
        /// </summary>
        /// <param name="ProjectMaster"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Save(ProjectMaster ProjectMaster)
        {
            var result = _IProjectRepository.Save(ProjectMaster);
            return Ok(ProjectMaster);
        }

        /// <summary>
        /// Get List of Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var projectdata = _IProjectRepository.GetAll("ProjectName", "Asc", "").ToList();
            return Ok(projectdata);
        }
        /// <summary>
        /// Get List of Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllByRole(string RoleCode)
        {
            var projectdata = _IProjectRepository.GetAllByRoleCode("ProjectName", "Asc", "", RoleCode).ToList();
            return Ok(projectdata);
        }

        /// <summary>
        /// Get List of Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetById(int ID)
        {
            var projectdata = _IProjectRepository.GetById(ID);
            return Ok(projectdata);
        }

        /// <summary>
        /// Delete project
        /// </summary>
        /// <param name="ID">Project Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Delete(string ID)
        {
            var isExistsinTimesheet = _IProjectRepository.CheckProjectIDExistsInTimesheet(Convert.ToInt32(ID));
            var isExistsinExpense = _IProjectRepository.CheckProjectIDExistsInExpense(Convert.ToInt32(ID));
            if (isExistsinTimesheet == false && isExistsinExpense == false)
            {
                var data = _IProjectRepository.Delete(Convert.ToInt32(ID));
                if (data > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// Check Project code
        /// </summary>
        /// <param name="ProjectCode">ProjectCode</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CheckProjectCodeExists(string ProjectCode)
        {
            var isProjectCodeExists = false;
            if (ProjectCode != null)
            {
                isProjectCodeExists = _IProjectRepository.CheckProjectCodeExists(ProjectCode);
            }
            if (isProjectCodeExists)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// Check Project Name
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CheckProjectNameExists(string ProjectName)
        {
            var isProjectNameExists = false;
            if (ProjectName != null)
            {
                isProjectNameExists = _IProjectRepository.CheckProjectNameExists(ProjectName);
            }

            if (isProjectNameExists)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }


        /// <summary>
        /// Save assigned Projects
        /// </summary>
        /// <param name="assignProjectsModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveAssignedProjects(AssignProjects assignProjectsModel)
        {
            var result = _IProjectRepository.SaveAssignedProjects(assignProjectsModel);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetUnAssignedProjects()
        {
            var result = _IProjectRepository.GetUnAssignedProjects();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAdminProjects(int AdminUserId)
        {
            var result = _IProjectRepository.GetAdminProjects(AdminUserId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAssignedProjects(int userId)
        {
            var result = _IProjectRepository.GetAssignedProjects(userId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult RemoveProject(int userId,int projectId)
        {
          
            var role = _IProjectRepository.RemoveProject(userId, projectId);
            return Ok(role);
        }

    }
}