using System;
using System.Linq;
using System.Web.Http;
using POC.ViewModels;
using POC.Repository.Interface;

namespace POC.Controllers
{
  
    public class AllTimeSheetController : ApiController
    {
     
        private readonly ITimeSheetRepository _ITimeSheetRepository;
        private readonly IProjectRepository _IProjectRepository;      
        private readonly ILogger _logger;
        
        public AllTimeSheetController(ITimeSheetRepository timesheetRepository, IProjectRepository projectRepository,ILogger logger)
        {
            _ITimeSheetRepository = timesheetRepository;
            _IProjectRepository = projectRepository;          
            this._logger = logger;
        }
        public IHttpActionResult LoadTimeSheetData(int userId)
        {
            try
            {
               
                int? start = 0, length = 0;
                string sortColumn = "", sortColumnDir = "", searchValue = "";
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var v = _ITimeSheetRepository.ShowTimeSheet(sortColumn, sortColumnDir, searchValue, userId);
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IHttpActionResult Details(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }
                MainTimeSheetView objMT = new MainTimeSheetView
                {
                    TimesheetStatus = _ITimeSheetRepository.GetTimesheetstatus(id),
                    TimeSheetMasterID = Convert.ToInt32(id)
                };
                return Ok(objMT);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
      
        public IHttpActionResult LoadSubmittedTimeSheetData(int userId)
        {
            try
            {
                var timesheetStatus = (int)TimeSheetStatus.Submit;
             
                int? start = 0, length = 0;
                string sortColumn = "", sortColumnDir = "", searchValue = "";
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;             
                var data = _ITimeSheetRepository.ShowTimeSheetStatus(sortColumn, sortColumnDir, searchValue, userId, timesheetStatus);               
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IHttpActionResult LoadRejectedTimeSheetData(int userId)
        {
            try
            {
                var timesheetStatus = (int)TimeSheetStatus.Reject;
                int? start = 0, length = 0;
                string sortColumn = "", sortColumnDir = "", searchValue = "";
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;                          
                var data = _ITimeSheetRepository.ShowTimeSheetStatus(sortColumn, sortColumnDir, searchValue, userId, timesheetStatus);              
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IHttpActionResult LoadApprovedTimeSheetData(int userId)
        {
            try
            {
                var timesheetStatus = (int)TimeSheetStatus.Approve;
                int? start = 0, length = 0;
                string sortColumn = "", sortColumnDir = "", searchValue = "";
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;              
                var data = _ITimeSheetRepository.ShowTimeSheetStatus(sortColumn, sortColumnDir, searchValue, userId, timesheetStatus);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}