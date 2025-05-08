using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IFranchiseeRoomMappingByUserIdUsecase
    {
        Task <List<FranchiseeRoomMappingResponse>> ExecuteAsync(int userId);
    }
}
