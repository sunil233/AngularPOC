using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
namespace POC.Repository.Implementation
{
    public class JobsRepository : IJobsRepository
    {

       
        public List<JobsTB> GetJobs()
        {
            using (var _context = new DatabaseContext())
            {
                var jobs = (from job in _context.Jobs
                             select job).ToList();
                return jobs;
            }
        }
      
    }
}
