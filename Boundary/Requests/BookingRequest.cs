using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Boundary.Requests
{
    public class BookingRequest
    {   
        public int ID { get; set; }
        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Franchisee Admin ID is required.")]
        public int FranchiseeAdminID { get; set; }
        [Required(ErrorMessage = "Room Event ID is required.")]
        public int RoomEventID { get; set; }
        [Required(ErrorMessage = "Room ID is required.")]
        public int RoomID { get; set; }
        [Required(ErrorMessage = "Booking Start Date is required.")]
        public DateTime BookingStartDateTime { get; set; }
        [Required(ErrorMessage = "Booking End Date is required.")]
        public DateTime BookingEndDateTime { get; set; }
        [Required(ErrorMessage = "Number of People is required.")]
        public int NumberofPeople { get; set; }
    }
}
