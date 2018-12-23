using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POC.Models
{
    [Table("Jobs")]
    public class JobsTB
    {
        [Key]
        public int JobId { get; set; }
        [Required(ErrorMessage = "Enter Job Title")]
        public string JobTitle { get; set; }
        public string JobCode { get; set; }
    }
}
