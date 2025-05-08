using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class UserGateway : BaseGateway<User>, IUserGateway
    {
        public UserGateway(ManyRoomStudioContext context , IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
