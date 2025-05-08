using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Domains.Rooms;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IRoomManagementUsecase
    {
        Task<RoomManagementDomain> ExecuteAsync();
    }
}
