using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IRoomSearchUsecase 
    {
        Task<List<FilterRoomResponse>> ExecuteAsync(RoomSearchRequest request);
    }
}
