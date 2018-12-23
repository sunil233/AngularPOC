
using POC.Repository.Interface;
using POC.Models;

namespace POC.Repository.Implementation
{
    public class AuditRepository : IAuditRepository
    {
        public void InsertAuditData(AuditTB audittb)
        {
            using (var _context = new DatabaseContext())
            {
                _context.AuditTB.Add(audittb);
                _context.SaveChanges();
            }
        }
    }
}
