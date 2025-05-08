namespace ManyRoomStudio.Boundary.Responses
{
    public class StudioBookResponse
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RoomEventID { get; set; }
        public string? Status { get; set; }
        public bool Iserror { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
