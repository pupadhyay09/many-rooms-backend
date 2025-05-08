using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Repository;
using ManyRoomStudio.Models.Entities;
namespace ManyRoomStudio.Gateways
{
    public class RoomFranchiseeAdminMappingGateway : BaseGateway<RoomFranchiseeAdminMapping>, IRoomFranchiseeAdminMappingGateway
    {

        public RoomFranchiseeAdminMappingGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {

        }
    }
}
