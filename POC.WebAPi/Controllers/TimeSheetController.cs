using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;
using POC.Models;
using POC.ViewModels;
using POC.Repository.Interface;

namespace POC.Controllers
{
    // [ValidateUserSession]
    public class TimeSheetController : ApiController
    {
       

        private readonly ITimeSheetRepository _ITimeSheetRepository;
        private readonly IProjectRepository _IProjectRepository;
        private readonly IUsersRepository _IUsersRepository;
        private readonly ITaskRepository _ITaskRepository;
        private readonly ILogger _logger;
        public TimeSheetController(ITimeSheetRepository timesheetRepository,
            IProjectRepository projectRepository,
            IUsersRepository userRepository,
            ITaskRepository taskRepository,ILogger logger)
        {
            _ITimeSheetRepository = timesheetRepository;
            _IProjectRepository = projectRepository;
            _IUsersRepository = userRepository;
            _ITaskRepository = taskRepository;
            this._logger = logger;
        }

        /// <summary>
        /// method to get list of projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ListofProjects()
        {
            try
            {
                var listofProjects = _IProjectRepository.GetListofProjects();
                return Ok(listofProjects);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// method to get list of tasks by project id
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ListofTasks(int ProjectId)
        {
            try
            {
                var listofTasks = _ITaskRepository.GetTasksByProjectId(ProjectId);
                return Ok(listofTasks);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Get TimeSheet by Userid
        /// </summary>
        /// <param name="userdata"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetTimeSheetForUserById(UserData userdata)
        {
            try
            {

                var FromDate = Convert.ToDateTime(userdata.Startdate, new CultureInfo("es-US", true));            
                var ToDate = Convert.ToDateTime(userdata.Enddate, new CultureInfo("es-US", true));
                var timesheetdetails = _ITimeSheetRepository.GetTimeSheetMasterIDTimeSheet(FromDate, ToDate, Convert.ToInt32(userdata.UserId));
                return Ok(timesheetdetails);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }


        }


        /// <summary>
        /// Method to Get TimeSheet by Userid
        /// </summary>
        /// <param name="userdata"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetTimeSheetByMasterId(int TimesheetMasterId)
        {
            try
            {
                var timesheetdetails = _ITimeSheetRepository.GetTimeSheetByMasterId(TimesheetMasterId);
                return Ok(timesheetdetails);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }


        }

        /// <summary>
        /// Method to save time sheet
        /// </summary>
        /// <param name="timesheetdata"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveTimeSheet(TimeSheetData timesheetdata)
        {
            try
            {
                var timesheetStatus = (int)TimeSheetStatus.Save;
                if (timesheetdata.ActionType == "SubmitToSupervisor")
                    timesheetStatus = (int)TimeSheetStatus.Submit;
                //if new time sheet
                if (timesheetdata.TimeSheetMasterID < 1)
                {
                    timesheetdata.TimeSheetMasterID = SaveTimeSheetMaster(timesheetdata.FromDate, timesheetdata.ToDate, timesheetdata.TotalHours, timesheetdata.Comments, timesheetStatus, timesheetdata.UserId);
                    _ITimeSheetRepository.InsertTimeSheetAuditLog(InsertTimeSheetAudit(timesheetdata.TimeSheetMasterID, timesheetStatus, timesheetdata.UserId));
                }
                else
                {
                    var timesheetMaster = new TimeSheetMaster()
                    {
                        TimeSheetMasterID = timesheetdata.TimeSheetMasterID,
                        TimeSheetStatus = timesheetStatus,
                        TotalHours = timesheetdata.TotalHours
                    };
                    _ITimeSheetRepository.UpdateTimeSheetMaster(timesheetMaster);
                }
                foreach (List<TimeSheetDetailsView> TimeSheetList in timesheetdata.TimeSheetList)
                {
                    foreach (TimeSheetDetailsView item in TimeSheetList)
                    {
                        if (item.TimeSheetID > 0)
                        {
                            var timeSheet = new TimeSheetDetails()
                            {
                                TimeSheetID = item.TimeSheetID,
                                TimeSheetMasterID = item.TimeSheetMasterID,
                                DaysofWeek = item.DaysofWeek,
                                Hours = item.Hours,
                                Period = Convert.ToDateTime(item.Period, new CultureInfo("es-US", true)),
                                ProjectID = item.ProjectID,
                                TaskId = item.TaskID
                            };
                            _ITimeSheetRepository.UpdateTimeSheetDetail(timeSheet);
                        }
                        else
                        {
                            if (item.Period != null && item.Period != "undefined")
                            {
                                var period = Convert.ToDateTime(item.Period, new CultureInfo("es-US", true));
                                SaveTimeSheetDetail(item.DaysofWeek, item.Hours, period, item.ProjectID, timesheetdata.TimeSheetMasterID, item.TaskID, timesheetdata.UserId);
                            }

                        }
                    }
                }
               
                if (timesheetdata.ActionType == "SubmitToSupervisor")
                {

                    _ITimeSheetRepository.UpdateTimeSheetAuditStatus(timesheetdata.TimeSheetMasterID, timesheetdata.Comments, timesheetStatus);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return NotFound();
            }
           
           
        }


        /// <summary>
        /// Method to delete time sheet
        /// </summary>
        /// <param name="TimeSheetMasterID">TimeSheetMasterID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Delete(int TimeSheetMasterID,int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(TimeSheetMasterID)))
                {
                    return NotFound();
                }

                var data = _ITimeSheetRepository.DeleteTimesheetByTimeSheetMasterID(TimeSheetMasterID, userId);

                if (data > 0)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to save time sheet
        /// </summary>
        /// <param name="DaysofWeek">DaysofWeek</param>
        /// <param name="Hours">Hours</param>
        /// <param name="Period">Period</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="TimeSheetMasterID">TimeSheetMasterID</param>
        private int SaveTimeSheetDetail(string DaysofWeek, int? Hours, DateTime? Period, int? ProjectID, int TimeSheetMasterID, int TaskId,int userid)
        {
            try
            {
                TimeSheetDetails objtimesheetdetails = new TimeSheetDetails();
                objtimesheetdetails.TimeSheetID = 0;
                objtimesheetdetails.DaysofWeek = DaysofWeek;
                objtimesheetdetails.Hours = Hours == null ? 0 : Hours;
                objtimesheetdetails.Period = Period;
                objtimesheetdetails.ProjectID = ProjectID;
                objtimesheetdetails.UserID = userid;
                objtimesheetdetails.CreatedOn = DateTime.Now;
                objtimesheetdetails.TimeSheetMasterID = TimeSheetMasterID;
                objtimesheetdetails.TaskId = TaskId;
                objtimesheetdetails.TimesheetStatus = (int)TimeSheetStatus.Save;
                int TimeSheetID = _ITimeSheetRepository.AddTimeSheetDetail(objtimesheetdetails);
                return TimeSheetID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to insert Time Sheet Audit
        /// </summary>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="TotalHours">TotalHours</param>
        /// <param name="Comment">Comment</param>
        /// <param name="TimesheetStatus">TimesheetStatus</param>
        /// <returns></returns>
        private int SaveTimeSheetMaster(DateTime FromDate, DateTime ToDate, int TotalHours, string Comment, int TimesheetStatus, int userid)
        {
            TimeSheetMaster objtimesheetmaster = new TimeSheetMaster();
            objtimesheetmaster.TimeSheetMasterID = 0;
            objtimesheetmaster.UserID = userid;
            objtimesheetmaster.CreatedOn = DateTime.Now;
            objtimesheetmaster.FromDate = FromDate;
            objtimesheetmaster.ToDate = ToDate;
            objtimesheetmaster.TotalHours = TotalHours;
            objtimesheetmaster.TimeSheetStatus = TimesheetStatus;
            objtimesheetmaster.Comment = Comment;
            int TimeSheetMasterID = _ITimeSheetRepository.AddTimeSheetMaster(objtimesheetmaster);
            return TimeSheetMasterID;
        }


        /// <summary>
        /// Method to insert time sheet audit
        /// </summary>
        /// <param name="TimeSheetMasterID">TimeSheetMasterID</param>
        /// <param name="Status">Status</param>
        /// <returns></returns>
        private TimeSheetAuditTB InsertTimeSheetAudit(int TimeSheetMasterID, int Status, int userid)
        {
            try
            {
                TimeSheetAuditTB objAuditTB = new TimeSheetAuditTB();
                objAuditTB.ApprovalTimeSheetLogID = 0;
                objAuditTB.TimeSheetMasterID = TimeSheetMasterID;
                objAuditTB.Status = Status;
                objAuditTB.CreatedOn = DateTime.Now;
                objAuditTB.Comment = string.Empty;
                objAuditTB.ApprovalUser = _IUsersRepository.GetAdminIDbyUserID(userid);
                objAuditTB.ProcessedDate = DateTime.Now;
                objAuditTB.UserID = userid;
                return objAuditTB;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class UserData
    {
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public int UserId { get; set; }
    }
    public class TimeSheetData
    {
        public  List<List<TimeSheetDetailsView>> TimeSheetList { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TimeSheetMasterID { get; set; }
        public int TotalHours { get; set; }
        public string ActionType { get; set; }
        public string Comments { get; set; }

        public int UserId { get; set; }
    }
}