using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class RoomGateway : BaseGateway<Room>, IRoomGateway
    {
        public RoomGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
