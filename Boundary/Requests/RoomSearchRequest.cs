namespace ManyRoomStudio.Boundary.Requests
{
    public class RoomSearchRequest
    {
        public DateTime? BookingDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int?  Numberofpeople{ get; set; }
        public int? EventType { get; set; }

    }
}
