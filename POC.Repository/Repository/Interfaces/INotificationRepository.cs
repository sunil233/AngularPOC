using System.Linq;
using POC.Models;

namespace POC.Repository.Interface
{
    public interface INotificationRepository
    {
        int AddNotification(NotificationsTB entity);
        void DisableExistingNotifications();
        IQueryable<NotificationsTB_ViewModel> ShowNotifications(string sortColumn, string sortColumnDir, string Search);

        bool DeActivateNotificationByID(int NotificationID);
    }
}
