using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.Domains.Staff
{
    public class AdminStaffDomain
    {

        public AdminStaffDomain()
        {
            this.franchiseeUserResponses = new List<RoomFranchiseeUserResponse>();
            this.staffuserResponses = new List<StaffModelResponse>();

        }
        public List<RoomFranchiseeUserResponse> franchiseeUserResponses { get; set; }

        public List<StaffModelResponse> staffuserResponses { get; set; }
    }
}
