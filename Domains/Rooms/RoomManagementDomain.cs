using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.Domains.Rooms
{
    public class RoomManagementDomain
    {
        public RoomManagementDomain() {
            this.eventType = new List<MasterDetailsResponse>();
            this.roomManagementResponses = new List<RoomManagementResponse>();

        }

        public List<MasterDetailsResponse> eventType { get; set; }
        public List<RoomManagementResponse>  roomManagementResponses { get; set; }

    }
}
