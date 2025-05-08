namespace ManyRoomStudio.Boundary.Requests
{
    public class AssignedRoomReqest
    {
        public int UserId { get; set; }
        public  List<int>? RoomIds { get; set; }
    }
}
