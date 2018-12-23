using System;
using System.Linq;
using System.Web.Http;
using POC.Models;
using POC.Repository.Interface;


namespace POC.Controllers
{

    public class AddNotificationController : ApiController
    {
        private readonly INotificationRepository _INotificationRepository;
        private readonly ILogger _logger;
        public AddNotificationController(INotificationRepository objrepository, ILogger logger)
        {
            this._INotificationRepository = objrepository;
            this._logger = logger;
        }

        [HttpPost]
        public IHttpActionResult Add(NotificationsTB NotificationsTB)
        {
            try
            {
                this._INotificationRepository.DisableExistingNotifications();
                var Notifications = new NotificationsTB
                {
                    CreatedOn = DateTime.Now,
                    Message = NotificationsTB.Message,
                    NotificationsID = 0,
                    Status = "A",
                    FromDate = NotificationsTB.FromDate,
                    ToDate = NotificationsTB.ToDate
                };
                this._INotificationRepository.AddNotification(Notifications);
                // MyNotificationHub.Send();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IHttpActionResult LoadNotificationData()
        {
            try
            {
                var draw = 0;
                int? start = 0, length = 0;
                string sortColumn = "", sortColumnDir = "", searchValue = "";
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var notificationdata = _INotificationRepository.ShowNotifications(sortColumn, sortColumnDir, searchValue);
                recordsTotal = notificationdata.Count();
                var data = notificationdata.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IHttpActionResult DeActivateNotification(string NotificationID)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(NotificationID)))
                {
                    return NotFound();
                }

                var result = this._INotificationRepository.DeActivateNotificationByID(Convert.ToInt32(NotificationID));

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}