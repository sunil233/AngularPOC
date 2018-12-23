using System.Linq;
using POC.Models;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IRegistrationRepository
    {
        bool CheckUserNameExists(string Username);
        int AddUser(Registration entity);
        int UpdateUser(Registration entity);
        IQueryable<RegistrationViewSummaryModel> ListofRegisteredUser(string sortColumn, string sortColumnDir, string Search);
        bool UpdatePassword(int RegistrationID, string Password);
        int CanDeleteUser(int RegistrationID);
        int DeleteUser(int RegistrationID);
    }
}
