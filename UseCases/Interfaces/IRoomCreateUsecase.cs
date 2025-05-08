using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IRoomCreateUsecase
    {
        Task<RoomResponse> ExecuteAsync(RoomRequest request);
    }
}
