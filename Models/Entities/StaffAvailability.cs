using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Models.Entities
{
    public class StaffAvailability :Auditable
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserStaffId { get; set; }
        public string? DaysName { get; set; }
        public string? DayTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
