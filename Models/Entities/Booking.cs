using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class Booking : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int FranchiseeAdminID { get; set; }
        public int RoomID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalVATAmount { get; set; }
        public int VATPercentage { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaybleAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RoomEventID { get; set; }
        public int NumberofPeople { get; set; }
        public string? Status { get; set; }
        public bool IsDelete { get; set; }
        public User CustomerDetail { get; set; }
        //public List<BookingRoom> BookingRoomList { get; set; }
    }
}
