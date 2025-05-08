using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;

namespace ManyRoomStudio.Gateways
{
    public class BookingGateway : BaseGateway<Booking>, IBookingGateway
    {
        public BookingGateway(ManyRoomStudioContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }
    }
}
