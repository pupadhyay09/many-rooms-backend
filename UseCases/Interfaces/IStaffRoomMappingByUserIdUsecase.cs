using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IStaffRoomMappingByUserIdUsecase
    {
        Task<List<StaffRoomMappingResponse>> ExecuteAsync(int staffid);
    }
}
