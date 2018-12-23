using System.Collections.Generic;
using POC.Models;
namespace POC.Repository.Interface
{
    public interface IJobsRepository
    {
        List<JobsTB> GetJobs();
    }
}
