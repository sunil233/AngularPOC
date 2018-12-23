using System;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using POC.Repository.Interface;

namespace POC.Controllers
{
    public class UserTimeSheetController : ApiController
    {
        private readonly ITimeSheetRepository _ITimeSheetRepository;
        private readonly IExpenseRepository _IExpenseRepository;
        private readonly ILogger _logger;
        public UserTimeSheetController(ITimeSheetRepository timesheetRepository, IExpenseRepository expenseRepository, ILogger logger)
        {
            _ITimeSheetRepository = timesheetRepository;
            _IExpenseRepository = expenseRepository;
            this._logger = logger;
        }

        [HttpGet]
        public IHttpActionResult GetTimeSheetsByStatus(int userId, int timesheetStatus)
        {
            var timesheetdata = _ITimeSheetRepository.ShowTimeSheetStatus("FromDate", "Desc", "", userId, timesheetStatus);
            var data = timesheetdata.ToList();
            return Ok(data);
        }
    }
}