using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class EmailSetupDetailsGateway : BaseGateway<EmailSetupDetail>, IEmailSetupDetailsGateway
    {
        public EmailSetupDetailsGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
