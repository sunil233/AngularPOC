using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.Models
{
    [Table("Documents")]
    public class Documents
    {
        [Key]
        public int DocumentID { get; set; }
        public int ProjectId { get; set; }
        public string DocumentTitle { get; set; }
        public int DocumentTypeId { get; set; }
        public string FileNameUrl { get; set; }
        public byte[] DocumentBytes { get; set; }
        public int AssignedToId { get; set; }        
        public int UploadedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DocumentDescription { get; set; }
        

    }
}
