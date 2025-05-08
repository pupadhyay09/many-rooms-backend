using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class EmailLog : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? ToEmail { get; set; }
        public string? FromEmail { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsSent { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsDelete { get; set; }
    }
}
