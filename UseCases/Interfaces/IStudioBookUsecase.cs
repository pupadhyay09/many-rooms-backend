using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IStudioBookUsecase
    {
        Task<StudioBookResponse> ExecuteAsync(BookingRequest request);
    }
}
