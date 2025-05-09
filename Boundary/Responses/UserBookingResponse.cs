using ManyRoomStudio.Models.Entities;

namespace ManyRoomStudio.Boundary.Responses
{
    public class UserBookingResponse :Auditable
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RoomEventID { get; set; }
        public string? Status { get; set; }
        public int RoomID { get; set; }


        //User Info
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
        public string? Country { get; set; }
        public string? Website { get; set; }
        public string? MobileNo { get; set; }
        public string Email { get; set; }


        //Room Info
        public string? RoomName { get; set; }
        public string? Description { get; set; }
        public int FranchiseeAdminID { get; set; }
        public decimal HourlyPrice { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public int Capacity { get; set; }
        public int TotalBeds { get; set; }
        public int TotalSofas { get; set; }
        public List<string> RoomImagePath { get; set; }
        public string RoomEventsName { get; set; }
    }
}
