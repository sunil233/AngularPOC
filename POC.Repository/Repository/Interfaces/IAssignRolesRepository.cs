using System.Collections.Generic;
using System.Linq;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IAssignRolesRepository
    {
        List<AdminModel> ListofAdmins();
        List<UserModel> ListofUser();
        IQueryable<UserModel> ShowallRoles(string sortColumn, string sortColumnDir, string Search);
        bool RemovefromUserRole(int RegistrationID);
        List<UserModel> GetListofUnAssignedUsers();
        bool SaveAssignedRoles(AssignRolesModel AssignRolesModel);
        bool CheckIsUserAssignedRole(int RegistrationID);
        bool AssignManager(AssignRolesModel AssignRolesModel);

    }

}