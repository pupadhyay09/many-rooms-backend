using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Domains.Staff;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IAdminStaffUsecase
    {
        Task <AdminStaffDomain> ExecuteAsync(int FranchiseeUserID);
    }
}
