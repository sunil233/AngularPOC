using POC.Models;
using POC.Repository.Interface;
using POC.ViewModels;
using System;
using System.Linq;
using System.Web.Http;


namespace POC.Controllers
{
  
    public class AllUsersController : ApiController
    {
        private readonly IRegistrationRepository _IRegistrationRepository;
        private readonly IRoleRepository _IRoleRepository;            
        private readonly IAssignRolesRepository _IAssignRolesRepository;
        private readonly IUsersRepository _IUsersRepository;
        private IDepartmentRepository _IDepartmentRepository;
        private IJobsRepository _IJobsRepository;       
        private readonly ILogger _logger;
        public AllUsersController(  IRegistrationRepository IRegistrationRepository,
                                    IRoleRepository IRoleRepository,
                                    IAssignRolesRepository IAssignRolesRepository,
                                    IUsersRepository IUsersRepository,
                                    IDepartmentRepository IDepartmentRepository,
                                    IJobsRepository IJobsRepository,
                                    ILogger logger)
        {
            _IRegistrationRepository = IRegistrationRepository;
            _IRoleRepository = IRoleRepository;
            _IAssignRolesRepository = IAssignRolesRepository;
            _IUsersRepository = IUsersRepository;
            _IDepartmentRepository = IDepartmentRepository;
            _IJobsRepository = IJobsRepository;
            _logger = logger;           
        }

        [HttpGet]
        public IHttpActionResult GetUserDetails()
        {
          
            var userDetailsResponse = new RegistrationViewDetailsModel();          
          
            var roles = _IRoleRepository.getRoles().Select(arg =>
                       new ValueDescription
                       {
                           Value = Convert.ToString(arg.RoleID),
                           Text = arg.Rolename

                       }).ToList();
            var departments = _IDepartmentRepository.GetDepartments()
                .Select(arg =>
                       new ValueDescription
                       {
                           Value = Convert.ToString(arg.DeptId),
                           Text = arg.DepartmentName

                       }).ToList();
            var jobs = _IJobsRepository.GetJobs()
                .Select(arg =>
                       new ValueDescription
                       {
                           Value = Convert.ToString(arg.JobId),
                           Text = arg.JobTitle

                       }).ToList();
            var managers = _IAssignRolesRepository.ListofAdmins().Select(n =>
                        new ValueDescription
                        {
                            Value = Convert.ToString(n.RegistrationID),
                            Text = n.FullName

                        }).ToList();
            if (userDetailsResponse != null)
            {
                userDetailsResponse.Roles = roles;
                userDetailsResponse.Departments = departments;
                userDetailsResponse.Jobs = jobs;
                userDetailsResponse.Managers = managers;

            }           
            return Ok(userDetailsResponse);
        }

        [HttpGet]
        public IHttpActionResult GetUserDetailsById(int? RegistrationID)
        {

            var userDetailsResponse = _IUsersRepository.GetUserDetailsByRegistrationID(RegistrationID);
            if (userDetailsResponse == null)
            {
                userDetailsResponse = new RegistrationViewDetailsModel();
            }
            userDetailsResponse.IsActive = (string.IsNullOrEmpty(userDetailsResponse.DateofLeaving) || userDetailsResponse.DateofLeaving == "01/01/0001") ? true : false;
            userDetailsResponse.DateofLeaving = (string.IsNullOrEmpty(userDetailsResponse.DateofLeaving) || userDetailsResponse.DateofLeaving == "01/01/0001") ? "" : userDetailsResponse.DateofLeaving;
            var RId = Convert.ToString(RegistrationID);
            var roles = _IRoleRepository.getRoles().Select(arg =>
                       new ValueDescription
                       {
                           Value = Convert.ToString(arg.RoleID),
                           Text = arg.Rolename

                       }).ToList();
            var departments = _IDepartmentRepository.GetDepartments()
                .Select(arg =>
                       new ValueDescription
                       {
                           Value = Convert.ToString(arg.DeptId),
                           Text = arg.DepartmentName

                       }).ToList();
            var jobs = _IJobsRepository.GetJobs()
                .Select(arg =>
                       new ValueDescription
                       {
                           Value = Convert.ToString(arg.JobId),
                           Text = arg.JobTitle

                       }).ToList();
            var managers = _IAssignRolesRepository.ListofAdmins().Select(n =>
                        new ValueDescription
                        {
                            Value = Convert.ToString(n.RegistrationID),
                            Text = n.FullName

                        }).ToList();
            if (userDetailsResponse != null)
            {
                userDetailsResponse.Roles = roles;
                userDetailsResponse.Departments = departments;
                userDetailsResponse.Jobs = jobs;
                userDetailsResponse.Managers = managers;

            }
            return Ok(userDetailsResponse);
        }

       
      
    }
}