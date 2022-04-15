using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewProject.Models
{
    public class Firm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FirmId { get; set; }
        public string FirmName { get; set; }
    }
}
