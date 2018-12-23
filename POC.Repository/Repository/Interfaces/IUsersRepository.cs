using System.Collections.Generic;
using System.Linq;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IUsersRepository
    {
        IQueryable<RegistrationViewSummaryModel> ShowallUsers(string sortColumn, string sortColumnDir, string Search);

        RegistrationViewDetailsModel GetUserDetailsByRegistrationID(int? RegistrationID);
        List<RegistrationViewSummaryModel> ShowallAdmin(string sortColumn, string sortColumnDir, string Search);

        RegistrationViewDetailsModel GetAdminDetailsByRegistrationID(int? RegistrationID);

        IQueryable<RegistrationViewSummaryModel> ShowallUsersUnderAdmin(string sortColumn, string sortColumnDir, string Search, int? RegistrationID);

        int GetTotalAdminsCount();
        int GetTotalUsersCount();
        int GetUserIDbyTimesheetID(int TimeSheetMasterID);
        int GetUserIDbyExpenseID(int ExpenseID);
        int GetAdminIDbyUserID(int UserID);
    }
}
