using POC.Repository.Interface;
using POC.ViewModels;
using System.Web.Http;

namespace POC.Controllers
{
    public class TaskController : ApiController
    {

        private readonly ITaskRepository _ITaskRepository;
        private readonly ILogger _logger;
        public TaskController(ITaskRepository ITaskRepository, ILogger logger)
        {
            _ITaskRepository = ITaskRepository;
            _logger = logger;
        }

        [HttpGet]
        public IHttpActionResult AddTasks(int ProjectId, string TaskName)
        {
            if (!string.IsNullOrEmpty(TaskName) && ProjectId > 0)
            {
                var task = new TaskViewModel()
                {
                    Taskname = TaskName,
                    ProjectID = ProjectId,
                    IsActive = true,
                    IsDeleted = false,
                    Status = 1
                };
                var taskId = _ITaskRepository.Save(task);
                if (taskId > 0)
                    return Ok(true);
                else
                    return Ok(false);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult Save(TaskViewModel taskModel)
        {
            var taskId = _ITaskRepository.Save(taskModel);
            if (taskId > 0)
                return Ok(true);
            else
                return Ok(false);
        }

        [HttpGet]
        public IHttpActionResult GetTasksByUserId(int ProjectId, int UserId)
        {
            var taskresult = _ITaskRepository.GetTasksByUserId(ProjectId, UserId);
            return Ok(taskresult);
        }

        [HttpGet]
        public IHttpActionResult IsTaskExists(int ProjectId, string TaskName)
        {
            if (!string.IsNullOrEmpty(TaskName) && ProjectId > 0)
            {
                var taskresult = _ITaskRepository.IsTaskExists(ProjectId, TaskName);
                return Ok(taskresult);
            }
            else
                return Ok(false);
        }

        [HttpGet]
        public IHttpActionResult GetTasksByProjectId(int ProjectId)
        {
            var taskresult = _ITaskRepository.GetTasksByProjectId(ProjectId);
            return Ok(taskresult);
        }

        [HttpGet]
        public IHttpActionResult GetById(int ID)
        {
            var taskresult = _ITaskRepository.GetById(ID);
            return Ok(taskresult);
        }
    }
}