using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class EmailSetupDetail : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FromEmail { get; set; }
        public int? Port { get; set; }
        public string Host { get; set; }
        public bool? EnableSsl { get; set; }
        public bool? UseDefaultCredentials { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDelete { get; set; }
    }
}
