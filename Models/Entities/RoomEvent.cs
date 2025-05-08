using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class RoomEvent : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int EventID { get; set; }
        public bool IsDelete { get; set; }
        public MasterDetail EventDetail { get; set; }
        public Room RoomDetail { get; set; }
    }
}
