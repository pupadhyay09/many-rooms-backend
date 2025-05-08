using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.EntitiesView;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class vwStaffModelGateway : BaseGateway<vwStaffModel>, IvwStaffModelGateway
    {
        public vwStaffModelGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
