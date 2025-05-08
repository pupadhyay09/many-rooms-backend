using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class RoomStaffMappingGateway : BaseGateway<RoomStaffMapping>, IRoomStaffMappingGateway
    {
        public RoomStaffMappingGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
