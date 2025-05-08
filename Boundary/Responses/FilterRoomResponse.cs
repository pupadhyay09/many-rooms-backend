namespace ManyRoomStudio.Boundary.Responses
{
    public class FilterRoomResponse
    {
        public int ID { get; set; }
        public string? Description { get; set; }
        public int FranchiseeAdminID { get; set; }
        public decimal HourlyPrice { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public int Capacity { get; set; }
        public int TotalBeds { get; set; }
        public int TotalSofas { get; set; }
        //public string FranchiseeName { get; set; }
        //public string FranchiseeCity { get; set; }
        //public string FranchiseeState { get; set; }
        //public string FranchiseePostcode { get; set; }
        //public string FranchiseeCountry { get; set; }
        //public string FranchiseeMobileNo { get; set; }
        public List<string> RoomImagePath { get; set; }
        public string RoomEventsName { get; set; }
    }
}
