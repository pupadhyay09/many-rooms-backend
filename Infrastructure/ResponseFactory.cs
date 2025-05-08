using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Models.EntitiesView;

namespace ManyRoomStudio.Infrastructure
{
    public static class ResponseFactory
    {
        public static RoomResponse ToResponse(this Room request)
        {
            return new RoomResponse
            {
                ID = request.ID,
                HourlyPrice = request.HourlyPrice,
                DiscountAmount = request.DiscountAmount,
                Capacity = request.Capacity,
                Iserror = false,
                ErrorMessage = ""
            };
        }

        public static FilterRoomResponse ToFilterResponse(this Room request)
        {
            return new FilterRoomResponse
            {
                ID = request.ID,
                HourlyPrice = request.HourlyPrice,
                DiscountAmount = request.DiscountAmount,
                Capacity = request.Capacity,
            };
        }

        public static MasterDetailsResponse ToResponse(this MasterDetail request)
        {
            return new MasterDetailsResponse
            {
                ID = request.ID,
                Category = request.Category,
                Name = request.Name,
                OtherName = request.OtherName,
                Description =   request.Description,
                ParentID =  request.ParentID,
            };
        }

        public static List<MasterDetailsResponse> ToResponse(this ICollection<MasterDetail> entity)
        {
            return entity?.Select(b => b.ToResponse()).ToList();
        }

        public static StudioBookResponse ToResponse(this Booking request)
        {
            return new StudioBookResponse
            {
                ID = request.ID,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                RoomEventID = request.RoomEventID,
                Status = request.Status,
                TotalAmount = request.TotalAmount,
                TotalDiscount = request.TotalDiscount,
                UserID = request.UserID,
                Iserror= false,
                ErrorMessage ="",
            };
        }


        public static UserLoginResponse ToResponse(this User request) {

            return new UserLoginResponse
            {
                ID = request.ID,
                FranchiseeAdminID = request.FranchiseeAdminID,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Postcode = request.Postcode,
                Country = request.Country,
                Website = request.Website,
                MobileNo = request.MobileNo,
                Email = request.Email,
                RoleID = request.RoleID,
                RolesName = Enum.GetName(typeof(ERole), request.RoleID) ?? "",

            };
        }

        public static UserModelResponse ToUserResponse(this User request)
        {

            return new UserModelResponse
            {
                ID = request.ID,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Postcode = request.Postcode,
                Country = request.Country,
                Website = request.Website,
                MobileNo = request.MobileNo,
                Email = request.Email,
                RolesName = Enum.GetName(typeof(ERole), request.RoleID) ?? "",
            };
        }

        public static List<UserModelResponse> ToUserResponse(this ICollection<User> entity)
        {
            return entity?.Select(b => b.ToUserResponse()).ToList();
        }


        public static StaffModelResponse TosttaffResponse(this vwStaffModel request)
        {

            return new StaffModelResponse
            {
                ID = request.ID,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Postcode = request.Postcode,
                Country = request.Country,
                Website = request.Website,
                MobileNo = request.MobileNo,
                Email = request.Email, 
                FranchiseeId = request.FranchiseeId != null ? (int)request.FranchiseeId :0,
                FranchiseeEmail = request.FranchiseeEmail,
                FranchiseeName = request.FranchiseeName,
            };
        }

        public static List<StaffModelResponse> TosttaffResponse(this ICollection<vwStaffModel> entity)
        {
            return entity?.Select(b => b.TosttaffResponse()).ToList();
        }        
    
        
    
    }
}
