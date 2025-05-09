namespace ManyRoomStudio.Boundary.Requests
{
    public class RoomRequest
    {
        public int ID { get; set; }
        public string? RoomName { get; set; }
        public string? Description { get; set; }
        public int FranchiseeAdminID { get; set; }
        public decimal HourlyPrice { get; set; }
        public int DiscountPercentage { get; set; }
        public int Capacity { get; set; }
        public List<int> RoomEventID { get; set; }
        public int TotalBeds { get; set; }
        public int TotalSofas { get; set; }
        public List<IFormFile>? File { get; set; }
        public List<int> RoomStaffID { get; set; }
        public int VATPercentage { get; set; }
        public int CommissionPercentage { get; set; }

    }
}
