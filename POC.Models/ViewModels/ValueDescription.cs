using System.ComponentModel.DataAnnotations.Schema;
namespace POC.Models
{
    [NotMapped]
    public class ValueDescription
    {
        public string Value { get; set; }
        public string Text { get; set; }

    }
}
