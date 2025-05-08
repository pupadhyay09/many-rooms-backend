using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class EmailTemplate : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? SystemName { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Keyword { get; set; }
        public string? FromEmail { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
