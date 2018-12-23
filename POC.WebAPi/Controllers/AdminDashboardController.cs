using System;
using System.Dynamic;
using System.Web.Http;
using POC.Repository.Interface;
using System.Linq;

namespace POC.Controllers
{
   public class AdminDashboardController : ApiController
    {
    
        private readonly ITimeSheetRepository _ITimeSheetRepository;
        private readonly IExpenseRepository _IExpenseRepository;
        private readonly IUsersRepository _IUsersRepository;
        private readonly IRoleRepository _IRoleRepository;
        private readonly ILogger _logger;

        public AdminDashboardController(ITimeSheetRepository timesheetRepository, 
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
        public IHttpActionResult Dashboard(string UserId)
        {
            try
            {
                var timesheetResult = _ITimeSheetRepository.GetTimeSheetsCountByAdminID(UserId);
                dynamic data = new ExpandoObject();
                if (timesheetResult != null)
                {
                   
                    data.SubmittedTimesheetCount = timesheetResult.SubmittedCount;
                    data.ApprovedTimesheetCount = timesheetResult.ApprovedCount;
                    data.RejectedTimesheetCount = timesheetResult.RejectedCount;
                }
                else
                {
                    data.SubmittedTimesheetCount = 0;
                    data.ApprovedTimesheetCount = 0;
                    data.RejectedTimesheetCount =0;
                }

                var expenseResult = _IExpenseRepository.GetExpenseAuditCountByAdminID(UserId);
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
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public IHttpActionResult GetTeamByAdminId(int? userId)
        {
            var adminusersdata = _IUsersRepository.ShowallUsersUnderAdmin("LastName", "Asc", "", userId).ToList();
            adminusersdata.ForEach(i => { i.RoleName = GetRoleName(i.RoleId); });
            return Ok(adminusersdata);
        }

        private string GetRoleName(int? RoleId)
        {
            return _IRoleRepository.GetRoleName(RoleId);
        }
    }
}