using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POC.Models
{
    [Table("DocumentType")]
    public class DocumentTypes
    {
        [Key]
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
