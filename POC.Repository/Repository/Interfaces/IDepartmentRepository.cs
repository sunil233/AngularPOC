using System.Collections.Generic;
using POC.Models;
namespace POC.Repository.Interface
{
    public interface IDepartmentRepository
    {

        List<DepartmentTB> GetDepartments();
    }
       
}
