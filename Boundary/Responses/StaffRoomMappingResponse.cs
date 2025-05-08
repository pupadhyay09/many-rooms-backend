namespace ManyRoomStudio.Boundary.Responses
{
    public class StaffRoomMappingResponse
    {
        public int RoomID { get; set; }
        public bool IsAssigned { get; set; }
        public string RoomName { get; set; }
        public int UserID { get; set; }
    }
}
