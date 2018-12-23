using POC.Helpers;
using POC.Models;
using POC.Repository.Interface;
using POC.ViewModels;
using System;
using System.Linq;
using System.Web.Http;
namespace POC.Controllers
{
    /// <summary>
    /// Api to get User details
    /// </summary>
    public class UserController : ApiController
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
        public UserController(IRegistrationRepository IRegistrationRepository,
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



        /// <summary>
        /// method to get All users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            var usersdata = _IUsersRepository.ShowallUsers("LastName", "Asc", "").ToList();
            usersdata.ForEach(i => { i.RoleName = GetRoleName(i.RoleId); });
            return Ok(usersdata);
        }

        /// <summary>
        /// method to get user details
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// method to get user details by user id
        /// </summary>
        /// <param name="RegistrationID"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserDetailsById(int? RegistrationID)
        {

            var userDetailsResponse = _IUsersRepository.GetUserDetailsByRegistrationID(RegistrationID);
            if (userDetailsResponse == null)
            {
                userDetailsResponse = new RegistrationViewDetailsModel();
            }
            if (string.IsNullOrEmpty(userDetailsResponse.DateofLeaving) || userDetailsResponse.DateofLeaving == "01/01/0001")
            {
                userDetailsResponse.IsActive = true;
            }
            else
            {
                userDetailsResponse.IsActive = false;

            }
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

        /// <summary>
        /// method to save user
        /// </summary>
        /// <param name="objuser"></param>
        /// <returns></returns>
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
                DateofLeaving = string.IsNullOrEmpty(objuser.DateofLeaving) ? (DateTime?)null : Convert.ToDateTime(objuser.DateofLeaving),
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
                //Assign Manager to the Employee
                var assignedManager = new AssignRolesModel
                {
                    AssignToAdmin = objuser.ManagerId,
                    RegistrationID = objuser.RegistrationID
                };
                var isAssigned = _IAssignRolesRepository.AssignManager(assignedManager);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(objuser.Username))
                {
                    var isvalidUserName = _IRegistrationRepository.CheckUserNameExists(objuser.Username);
                    if (!isvalidUserName)
                    {
                        result = _IRegistrationRepository.AddUser(user);
                        //Assign Manager to the Employee
                        var assignedManager = new AssignRolesModel
                        {
                            AssignToAdmin = objuser.ManagerId,
                            RegistrationID = result
                        };
                       var isAssigned= _IAssignRolesRepository.AssignManager(assignedManager);
                    }
                    else
                    {
                        result = -1;
                    }
                }
            }
            return Ok(result);
        }

        /// <summary>
        /// method to get role name by roleId
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        private string GetRoleName(int? RoleId)
        {
            return _IRoleRepository.GetRoleName(RoleId);
        }

        /// <summary>
        /// Method to Get Admin Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAdminUsers()
        {
            var admins = _IAssignRolesRepository.ListofAdmins();
            return Ok(admins);
        }

        /// <summary>
        /// Method to  Get List of UnAssigned Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetListofUnAssignedUsers()
        {
            var users = _IAssignRolesRepository.GetListofUnAssignedUsers();
            return Ok(users);
        }

        [HttpPost]
        public IHttpActionResult SaveAssignedRoles(AssignRolesModel assignRolesModel)
        {
            var result = _IAssignRolesRepository.SaveAssignedRoles(assignRolesModel);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetTeamMembers(int AdminId)
        {
            var team = _IUsersRepository.ShowallUsersUnderAdmin("LastName", "Asc", "", AdminId).ToList();
            return Ok(team);
        }        

    }

}