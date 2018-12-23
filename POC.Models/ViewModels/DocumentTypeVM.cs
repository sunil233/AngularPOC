using System;

namespace POC.ViewModels
{
    public class DocumentTypeVM
    {
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
