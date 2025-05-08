using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class BookingRoom : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public int BookingID { get; set; }
        public decimal RoomTotal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDelete { get; set; }
        public Room RoomDetail { get; set; }
       // public Booking BookingDetail { get; set; }
    }
}
