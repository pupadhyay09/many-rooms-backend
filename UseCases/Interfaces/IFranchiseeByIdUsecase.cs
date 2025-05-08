using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IFranchiseeByIdUsecase
    {
        Task<FranchiseeUserModelResponse> ExecuteAsync(int Id);
    }
}
