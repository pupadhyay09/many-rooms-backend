using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class MasterDetailsGateway : BaseGateway<MasterDetail>, IMasterDetailsGateway
    {
        public MasterDetailsGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
