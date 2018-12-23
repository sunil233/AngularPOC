using System.Collections.Generic;
using POC.Models;
using POC.ViewModels;

namespace POC.Repository.Interface
{
    public interface IDocumentRepository
    {
        int Add(Documents Documents);
        Documents GetById(long DocumentId);
        List<DocumentsVM> GetAll(long DocumentId, int userId);
        bool Delete(long DocumentId);
        byte[] GetAttachmentById(long DocumentId);
        byte[] Generate_SSRSReport(string reportName, string reporttype, List<dynamic> parameters);
    }
}
