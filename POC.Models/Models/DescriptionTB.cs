using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.Models
{
    [Table("DescriptionTB")]
    public class DescriptionTB
    {
        [Key]
        public int DescriptionID { get; set; }
        public string Description { get; set; }
        public int? ProjectID { get; set; }
        public int? TimeSheetMasterID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UserID { get; set; }

    }
}
