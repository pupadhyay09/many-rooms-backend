namespace ManyRoomStudio.Boundary.Responses
{
    public class RoomManagementResponse
    {
        public int ID { get; set; }
        public string? RoomName { get; set; }
        public string? Description { get; set; }
        public decimal HourlyPrice { get; set; }
        public int DiscountPercentage { get; set; }
        public int VATPercentage { get; set; }
        public int CommissionPercentage { get; set; }
        public int Capacity { get; set; }
        public int TotalBeds { get; set; }
        public int TotalSofas { get; set; }
        public string? OwnershipTypeName { get; set; }
        public string? RoomEventsName { get; set; }
        public bool IsVATEnabled { get; set; }
    }
}
