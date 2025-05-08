using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IUserBookingUsecase
    {
        Task <List<UserBookingResponse>> ExecuteAsync(int userId);
    }
}
