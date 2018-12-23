using POC.Models;
using POC.Repository.Interface;
using POC.ViewModels;
using System;
using System.Linq;
using System.Web.Http;

namespace POC.Controllers
{
    public class AdminTimeSheetController : ApiController
    {

        private readonly ITimeSheetRepository _ITimeSheetRepository;
        private readonly IExpenseRepository _IExpenseRepository;
        private readonly IUsersRepository _IUsersRepository;
        private readonly IRoleRepository _IRoleRepository;
        private readonly ILogger _logger;

        public AdminTimeSheetController(ITimeSheetRepository timesheetRepository,
            IExpenseRepository expenseRepository,
            IUsersRepository usersRepository,
            IRoleRepository roleRepository,
            ILogger logger)
        {
            _ITimeSheetRepository = timesheetRepository;
            _IExpenseRepository = expenseRepository;
            _IUsersRepository = usersRepository;
            _IRoleRepository = roleRepository;
            _logger = logger;
        }

        [HttpGet]
        public IHttpActionResult GetTimeSheetsByStatus(int userId,int timesheetStatus)
        {
            var timesheetdata = _ITimeSheetRepository.ShowAllTimeSheet("LastName", "Asc", "", userId, timesheetStatus).ToList();
            return Ok(timesheetdata);
        }

        [HttpGet]
        public IHttpActionResult LoadTimeSheetData(int userId, int timesheetStatus)
        {
            var timesheetdata = _ITimeSheetRepository.ShowTimeSheetStatus("LastName", "Asc", "", userId, timesheetStatus).ToList();
            return Ok(timesheetdata);
        }
        [HttpGet]
        public IHttpActionResult GetTimeSheetByMasterId(int TimesheetMasterId)
        {
            var timesheetdetails = _ITimeSheetRepository.GetTimeSheetByMasterId(TimesheetMasterId);
            return Ok(timesheetdetails);
        }
        [HttpPost]
        public IHttpActionResult Rejected(TimeSheetApproval TimeSheetApproval)
        {
            var timesheetStatus = (int)TimeSheetStatus.Reject;
            if (TimeSheetApproval.Comment == null)
            {
                return Json(false);
            }
            if (string.IsNullOrEmpty(Convert.ToString(TimeSheetApproval.TimeSheetMasterID)))
            {
                return NotFound();
            }
            _ITimeSheetRepository.UpdateTimeSheetStatus(TimeSheetApproval, timesheetStatus); //Reject

            if (_ITimeSheetRepository.IsTimesheetALreadyProcessed(TimeSheetApproval.TimeSheetMasterID))
            {
                _ITimeSheetRepository.UpdateTimeSheetAuditStatus(TimeSheetApproval.TimeSheetMasterID, TimeSheetApproval.Comment, timesheetStatus);
            }
            else
            {
                _ITimeSheetRepository.InsertTimeSheetAuditLog(InsertTimeSheetAudit(TimeSheetApproval, timesheetStatus));
            }
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult Approval(TimeSheetApproval TimeSheetApproval)
        {
            var timesheetStatus = (int)TimeSheetStatus.Approve;
            if (TimeSheetApproval.Comment == null)
            {
                return Json(false);
            }

            if (string.IsNullOrEmpty(Convert.ToString(TimeSheetApproval.TimeSheetMasterID)))
            {
                return Json(false);
            }

            _ITimeSheetRepository.UpdateTimeSheetStatus(TimeSheetApproval, 2); //Approve

            if (_ITimeSheetRepository.IsTimesheetALreadyProcessed(TimeSheetApproval.TimeSheetMasterID))
            {
                _ITimeSheetRepository.UpdateTimeSheetAuditStatus(TimeSheetApproval.TimeSheetMasterID, TimeSheetApproval.Comment, timesheetStatus);
            }
            else
            {
                _ITimeSheetRepository.InsertTimeSheetAuditLog(InsertTimeSheetAudit(TimeSheetApproval, timesheetStatus));
            }
            return Ok();
        }

        private TimeSheetAuditTB InsertTimeSheetAudit(TimeSheetApproval TimeSheetApproval, int Status)
        {
            try
            {
                TimeSheetAuditTB objAuditTB = new TimeSheetAuditTB();
                objAuditTB.ApprovalTimeSheetLogID = 0;
                objAuditTB.TimeSheetMasterID = TimeSheetApproval.TimeSheetMasterID;
                objAuditTB.Status = Status;
                objAuditTB.CreatedOn = DateTime.Now;
                objAuditTB.Comment = TimeSheetApproval.Comment;
                objAuditTB.ApprovalUser = Convert.ToInt32(TimeSheetApproval.UserId);
                objAuditTB.ProcessedDate = DateTime.Now;
                objAuditTB.UserID = _IUsersRepository.GetUserIDbyTimesheetID(TimeSheetApproval.TimeSheetMasterID);
                return objAuditTB;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}