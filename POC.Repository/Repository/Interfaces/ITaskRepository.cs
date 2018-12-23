using System.Collections.Generic;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface ITaskRepository
    {
       
        int Save(TaskViewModel taskModel);
        TaskViewModel GetById(int ID);
        bool IsTaskExists(int ProjectId, string TaskName);
        List<TaskViewModel> GetTasksByProjectId(int ProjectId);
        List<TaskViewModel> GetTasksByUserId(int ProjectId, int UserId);
       
    }
}
