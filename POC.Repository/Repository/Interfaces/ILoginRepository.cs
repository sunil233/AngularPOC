using POC.Models;

namespace POC.Repository.Interface
{
    public interface ILoginRepository
    {
        Registration ValidateUser(string userName, string passWord);
        bool UpdatePassword(string NewPassword, int UserID);
        string GetPasswordbyUserID(int UserID);
    }
}
