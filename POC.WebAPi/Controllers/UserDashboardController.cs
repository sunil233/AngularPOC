using System;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using POC.Repository.Interface;

namespace POC.Controllers
{
    public class UserDashboardController : ApiController
    {

        private readonly ITimeSheetRepository _ITimeSheetRepository;
        private readonly IExpenseRepository _IExpenseRepository;
        private readonly ILogger _logger;
        public UserDashboardController(ITimeSheetRepository timesheetRepository, IExpenseRepository expenseRepository, ILogger logger)
        {
            _ITimeSheetRepository = timesheetRepository;
            _IExpenseRepository = expenseRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get Dahboard data
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Dashboard(string userId)
        {
            var timesheetResult = _ITimeSheetRepository.GetTimeSheetsCountByUserID(userId);
            dynamic data = new ExpandoObject();
            if (timesheetResult != null)
            {
                data.RecentSubmissionsCount = (timesheetResult.SubmittedCount+ timesheetResult.ApprovedCount+ timesheetResult.RejectedCount+ timesheetResult.SaveddCount) >5?5: (timesheetResult.SubmittedCount + timesheetResult.ApprovedCount + timesheetResult.RejectedCount + timesheetResult.SaveddCount);
                data.SavedTimesheetCount = timesheetResult.SaveddCount;
                data.SubmittedTimesheetCount = timesheetResult.SubmittedCount;
                data.ApprovedTimesheetCount = timesheetResult.ApprovedCount;
                data.RejectedTimesheetCount = timesheetResult.RejectedCount;
            }
            else
            {
                data.SubmittedTimesheetCount = 0;
                data.ApprovedTimesheetCount = 0;
                data.RejectedTimesheetCount = 0;
            }


            var expenseResult = _IExpenseRepository.GetExpenseAuditCountByUserID(userId);

            dynamic expensedata = new ExpandoObject();
            if (expenseResult != null)
            {
                expensedata.SubmittedExpenseCount = expenseResult.SubmittedCount;
                expensedata.ApprovedExpenseCount = expenseResult.ApprovedCount;
                expensedata.RejectedExpenseCount = expenseResult.RejectedCount;
            }
            else
            {
                expensedata.SubmittedExpenseCount = 0;
                expensedata.ApprovedExpenseCount = 0;
                expensedata.RejectedExpenseCount = 0;
            }
            dynamic dathboarddata = new ExpandoObject();
            dathboarddata.TimesheetData = data;
            dathboarddata.ExpenseData = expensedata;
            return Ok(dathboarddata);
        }

        /// <summary>
        /// Get Recent timesheets
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult LoadRecentTimeSheets(int userId)
        {
            try
            {

                var timesheetdata = _ITimeSheetRepository.ShowMyrecentSubmittedTimeSheet("FromDate", "Desc", "", userId);
                var data = timesheetdata.Take(5).ToList();
                return Ok(data);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get Timesheets by status
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="timesheetStatus">timesheetStatus</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetTimeSheetsByStatus(int userId, int timesheetStatus)
        {
            var timesheetdata = _ITimeSheetRepository.ShowTimeSheetStatus("FromDate", "Desc", "", userId, timesheetStatus);
            var data = timesheetdata.ToList();
            return Ok(data);
        }
    }
}