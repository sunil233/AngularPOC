using System;
namespace POC.ViewModels
{
    
    public class DocumentsVM
    {
        public int DocumentID { get; set; }
        public int ProjectId { get; set; }
        public string DocumentTitle { get; set; }
        public string ProjectName { get; set; }
        public string DocumentType { get; set; }
        public string FileNameUrl { get; set; }
        public byte[] DocumentBytes { get; set; }
        public int AssignedToId { get; set; }
        public int UploadeById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DocumentDescription { get; set; }
    }
}
