using System;

using System.Web.Http;


namespace WebTimeSheetManagement.Controllers
{
    [ValidateUserSession]
    public class NotificationController : ApiController
    {
        public JsonResult GetNotification()
        {
            try
            {
                return Json(NotificationService.GetNotification(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}