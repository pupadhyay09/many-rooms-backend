using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Models.Entities
{
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ControllerName { get; set; } = null!;

        public string ActionName { get; set; } = null!;

        public string ExceptionType { get; set; } = null!;

        public string? ExceptionMessage { get; set; }

        public DateTime Timex { get; set; }

        public string? Ipaddress { get; set; }
    }
}
