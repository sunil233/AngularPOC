using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
namespace POC.Repository.Implementation
{
    public class DepartmentRepository : IDepartmentRepository
    {

        public List<DepartmentTB> GetDepartments()
        {
            using (var _context = new DatabaseContext())
            {
                var departments = (from department in _context.Departments
                             select department).ToList();
                return departments;
            }
        }
    }
}
