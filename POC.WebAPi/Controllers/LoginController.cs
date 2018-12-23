using POC.Library;
using POC.Repository.Interface;
using POC.ViewModels;
using System.Web.Http;

namespace POC.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IUsersRepository UsersRepository;
        private readonly ILoginRepository LoginRepository;
        private readonly IAssignRolesRepository AssignRolesRepository;
        private readonly IRoleRepository RoleRepository;
        private readonly ILogger Logger;
        public LoginController(IUsersRepository userRepository, ILoginRepository loginRepository, IAssignRolesRepository assignRolesRepository, IRoleRepository roleRepository, ILogger logger)
        {
            UsersRepository = userRepository;
            AssignRolesRepository = assignRolesRepository;
            RoleRepository = roleRepository;
            LoginRepository = loginRepository;
            Logger = logger;
        }    

        [HttpPost]     
        public IHttpActionResult Login(LoginViewModel loginViewModel)
        {
            if (!string.IsNullOrWhiteSpace(loginViewModel.Username) && !string.IsNullOrWhiteSpace(loginViewModel.Password))
            {
                var Username = loginViewModel.Username;
                var password = EncryptionLibrary.EncryptText(loginViewModel.Password);
                var result = LoginRepository.ValidateUser(Username, password);
                if (result != null)
                {
                    var RoleID = result.RoleID;
                    var roleCode = RoleRepository.GetRoleCode(RoleID);
                    loginViewModel.RoleCode = roleCode;
                    loginViewModel.UserId = result.RegistrationID;
                    if (!AssignRolesRepository.CheckIsUserAssignedRole(result.RegistrationID))
                    {
                        loginViewModel.IsAssigned = false;
                    }
                    else
                    {
                        loginViewModel.IsAssigned = true;
                    }
                }
            }
            return Ok(loginViewModel);
        }

      

    }
}
