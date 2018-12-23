using System.Collections.Generic;
using POC.Models;
namespace POC.Repository.Interface
{
    public interface IRoleRepository
    {
        int getRolesofUserbyRolename(string Rolename);
        List<Roles> getRoles();
        int AddRole(Roles role);
        int UpdateRole(Roles role);
        int DeleteRole(int ID);
        bool IsRoleExist(string RoleName);
        string GetRoleName(int? RoleId);
        string GetRoleCode(int? RoleId);
        Roles GetById(int ID);
        bool CheckRoleCodeExists(string RoleCode);
        bool CheckRoleNameExists(string RoleName);
    }
}
