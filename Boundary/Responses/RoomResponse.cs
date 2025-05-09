namespace ManyRoomStudio.Boundary.Responses
{
    public class RoomResponse
    {
        public int ID { get; set; }
        public string? Description { get; set; }
        public int FranchiseeAdminID { get; set; }
        public decimal HourlyPrice { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public int Capacity { get; set; }
        public int RoomEventID { get; set; }

        public bool Iserror { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
