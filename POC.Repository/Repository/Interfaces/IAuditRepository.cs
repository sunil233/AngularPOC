
using POC.Models;

namespace POC.Repository.Interface
{
    public interface IAuditRepository
    {
        void InsertAuditData(AuditTB audittb);
    }
}
