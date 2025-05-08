using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class ErrorGateway : BaseGateway<ErrorLog>, IErrorGateway
    {
        public ErrorGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
