using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Models.Entities;

namespace ManyRoomStudio.Infrastructure
{
    public static class EntityFactory
    {
        public static Room ToEntity(this RoomRequest request) {

            return new Room {
                ID = request.ID,
                HourlyPrice = request.HourlyPrice,
                DiscountAmount = request.DiscountAmount,
                Capacity = request.Capacity,
                IsDelete = false,
                Description = request.Description,
                TotalBeds = request.TotalBeds,
                TotalSofas = request.TotalSofas,
                RoomName = request.RoomName,
                VATPercentage = request.VATPercentage,
                CommissionPercentage = request.CommissionPercentage
            };
        }

        public static Booking ToEntity(this BookingRequest request)
        {
            return new Booking
            {
                ID = request.ID,
                FranchiseeAdminID = request.FranchiseeAdminID,
                RoomID = request.RoomID,
                StartDate = request.BookingStartDateTime,
                EndDate = request.BookingEndDateTime,
                NumberofPeople = request.NumberofPeople,
                UserID = request.UserID,
                RoomEventID = request.RoomEventID,
                IsDelete = false,
                CreatedBy = request.UserID,
                UpdatedBy = request.UserID,
            };
        }

        public static User ToEntity(this CreateStaffUserRequest request)
        {
            return new User
            {
                ID = request.ID,
                FranchiseeAdminID = request.FranchiseeAdminID,
                IsDelete = false,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address= request.Address,
                City =  request.City,
                State = request.State,
                Postcode= request.Postcode,
                Country = request.Country,
                Website = request.Website,
                MobileNo = request.MobileNo,
                Email = request.Email,
            };
        }

        public static User ToEntity(this UpdateStaffUserRequest request)
        {
            return new User
            {
                ID = request.ID,
                IsDelete = false,
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
            };
        }


        public static User ToEntity(this CreateFranchiseeUserRequest request)
        {
            return new User
            {
                ID = request.ID,
                FranchiseeAdminID = 0,
                IsDelete = false,
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
            };
        }
    }
}
