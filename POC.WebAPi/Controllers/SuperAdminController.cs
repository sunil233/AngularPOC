
using POC.Helpers;
using POC.Models;
using POC.Repository.Interface;
using POC.ViewModels;
using System;
using System.Dynamic;
using System.Linq;
using System.Web.Http;

namespace POC.Controllers
{

    public class SuperAdminController : ApiController
    {


        private readonly IRegistrationRepository _IRegistrationRepository;
        private readonly IRoleRepository _IRoleRepository;
        private readonly IAssignRolesRepository _IAssignRolesRepository;
        private readonly IUsersRepository _IUsersRepository;
        private readonly IProjectRepository _IProjectRepository;
        private IDepartmentRepository _IDepartmentRepository;
        private IJobsRepository _IJobsRepository;
        private readonly ILogger _logger;
        private readonly ICacheManager _ICacheManager;
        public SuperAdminController(IRegistrationRepository IRegistrationRepository,
                                    IRoleRepository IRoleRepository,
                                    IAssignRolesRepository IAssignRolesRepository,
                                    IUsersRepository IUsersRepository,
                                    IProjectRepository IProjectRepository,
                                    IDepartmentRepository IDepartmentRepository,
                                    IJobsRepository IJobsRepository,
                                    ILogger logger)
        {
            _IRegistrationRepository = IRegistrationRepository;
            _IRoleRepository = IRoleRepository;
            _IAssignRolesRepository = IAssignRolesRepository;
            _IUsersRepository = IUsersRepository;
            _IProjectRepository = IProjectRepository;
            _IDepartmentRepository = IDepartmentRepository;
            _IJobsRepository = IJobsRepository;
            _logger = logger;
            _ICacheManager = new CacheManager();
        }


        [HttpGet]
        public IHttpActionResult Dashboard()
        {
            var adminCount = _ICacheManager.Get<object>("AdminCount");
            if (adminCount == null)
            {
                var admincount = _IUsersRepository.GetTotalAdminsCount();
                _ICacheManager.Add("AdminCount", admincount);

            }
            var usersCount = _ICacheManager.Get<object>("UsersCount");
            if (usersCount == null)
            {
                var userscount = _IUsersRepository.GetTotalUsersCount();
                _ICacheManager.Add("UsersCount", userscount);

            }
            var projectCount = _ICacheManager.Get<object>("ProjectCount");
            if (projectCount == null)
            {
                var projectcount = _IProjectRepository.GetTotalProjectsCounts();
                _ICacheManager.Add("ProjectCount", projectcount);

            }
            dynamic dathboarddata = new ExpandoObject();
            dathboarddata.adminCount = adminCount;
            dathboarddata.usersCount = usersCount;
            dathboarddata.projectCount = projectCount;
            return Ok(dathboarddata);
        }

        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            var usersdata = _IUsersRepository.ShowallUsers("LastName", "Asc", "").ToList();
            usersdata.ForEach(i => { i.RoleName = GetRoleName(i.RoleId); });
            return Ok(usersdata);
        }

        [HttpGet]
        public IHttpActionResult GetAllAdminUsers()
        {
            var adminusersdata = _IUsersRepository.ShowallAdmin("LastName", "Asc", "");
            return Ok(adminusersdata);
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

        [HttpPost]
        public IHttpActionResult SaveUser(RegistrationViewDetailsModel objuser)
        {
            int result = 0;
            var user = new Registration()
            {
                FirstName = objuser.FirstName,
                MiddleName = objuser.MiddleName,
                LastName = objuser.LastName,
                Birthdate = Convert.ToDateTime(objuser.Birthdate),
                ConfirmPassword = objuser.ConfirmPassword,
                DateofJoining = Convert.ToDateTime(objuser.DateofJoining),
                DateofLeaving = Convert.ToDateTime(objuser.DateofLeaving),
                DeptId = objuser.DeptId,
                EmailID = objuser.EmailID,
                EmergencyContact = objuser.EmergencyContact,
                EmergencyContactNumber = objuser.EmergencyContactNumber,
                EmployeeID = objuser.EmployeeID,
                JobId = objuser.JobId,
                Gender = objuser.Gender,
                IsActive = objuser.IsActive ? true : true,
                ManagerId = objuser.ManagerId,
                Mobileno = objuser.Mobileno,
                Password = objuser.Password,
                RegistrationID = objuser.RegistrationID,
                RoleID = objuser.RoleID,
                Username = objuser.Username,
                WorkEmail = objuser.WorkEmail,
                CreatedOn = DateTime.Now
            };
            if (objuser.RegistrationID > 0)
            {
                result = _IRegistrationRepository.UpdateUser(user);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(objuser.Username))
                {
                    var isvalidUserName = _IRegistrationRepository.CheckUserNameExists(objuser.Username);
                    if (!isvalidUserName)
                    {
                        result = _IRegistrationRepository.AddUser(user);
                    }
                    else
                    {
                        result = -1;
                    }
                }
            }
            return Ok(result);
        }

        private string GetRoleName(int? RoleId)
        {
            return _IRoleRepository.GetRoleName(RoleId);
        }

    }

}