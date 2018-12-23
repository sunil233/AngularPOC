using POC.Models;
using POC.Repository.Interface;
using POCServices.Filters;
using System;
using System.Linq;
using System.Web.Http;

namespace POC.Controllers
{

    public class RolesController : ApiController
    {
        private readonly IRoleRepository _IRoleRepository;
        private readonly IAssignRolesRepository _IAssignRolesRepository;

        public RolesController(IRoleRepository IRoleRepository, IAssignRolesRepository IAssignRolesRepository)
        {
            _IRoleRepository = IRoleRepository;
            _IAssignRolesRepository = IAssignRolesRepository;
        }

        [HttpPost]
        public IHttpActionResult Save(Roles objroles)
        {
            var result = 0;
            if (objroles.RoleID > 0)
            {
                result = _IRoleRepository.UpdateRole(objroles);
            }
            else
            {
                var isroleexists = _IRoleRepository.IsRoleExist(objroles.Rolename);
                if (!isroleexists)
                {
                    result = _IRoleRepository.AddRole(objroles);
                }

            }
            return Ok(objroles);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var rolesData = _IRoleRepository.getRoles();
            return Ok(rolesData);
        }

        [HttpGet]
        public IHttpActionResult GetById(int ID)
        {
            var rolesData = _IRoleRepository.GetById(ID);
            return Ok(rolesData);
        }

        [HttpGet]
        public IHttpActionResult Delete(int ID)
        {
            if (ID < 1)
            {
                throw new POCException("InValid RoleID");
            }
            else
            {
                var role = _IRoleRepository.DeleteRole(ID);
                return Ok(role);
            }
        }

        [HttpGet]
        public IHttpActionResult ShowallRoles()
        {
            var rolesData = _IAssignRolesRepository.ShowallRoles("LastName", "ASC", "").ToList();
            return Ok(rolesData);
        }

        [HttpGet]
        public IHttpActionResult RemovefromRole(string RegistrationID)
        {
            if (string.IsNullOrEmpty(RegistrationID))
            {
                throw new POCException("RegistrationID Cannot be Null.");
            }
            else
            {
                var id = Convert.ToInt32(RegistrationID);
                var role = _IAssignRolesRepository.RemovefromUserRole(id);
                return Ok(role);
            }
        }

        [HttpGet]
        public IHttpActionResult CheckRoleCodeExists(string RoleCode)
        {
            var isProjectCodeExists = false;
            if (RoleCode != null)
            {
                isProjectCodeExists = _IRoleRepository.CheckRoleCodeExists(RoleCode);
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
        /// Check RoleName Name
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CheckRoleNameExists(string RoleName)
        {
            var isProjectNameExists = false;
            if (RoleName != null)
            {
                isProjectNameExists = _IRoleRepository.CheckRoleNameExists(RoleName);
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

    }
}