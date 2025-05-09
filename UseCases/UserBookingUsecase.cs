using Azure.Core;
using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class UserBookingUsecase : IUserBookingUsecase
    {
        private readonly IBookingGateway _bookingGateway;
        private readonly IRoomGateway _roomGateway;
        public UserBookingUsecase(IBookingGateway bookingGateway, IRoomGateway roomGateway)
        {
            _bookingGateway = bookingGateway;
            _roomGateway = roomGateway;
        }
        public async Task<List<UserBookingResponse>> ExecuteAsync(int userId)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {

                var bookingsQuery = from b in db.Bookings.AsNoTracking()
                                    join u in db.Users.AsNoTracking() on b.UserID equals u.ID
                                    join r in db.Rooms.AsNoTracking().Include(x => x.RoomImages).Where(x=>x.IsDelete == false) on b.RoomID equals r.ID
                                    join re in db.MasterDetails.AsNoTracking() on b.RoomEventID equals re.ID
                                    where !b.IsDelete
                                    orderby b.UpdatedAt descending
                                    select new UserBookingResponse
                                    {
                                        ID = b.ID,
                                        UserID = b.UserID,
                                        TotalAmount = b.TotalAmount,
                                        TotalDiscount = b.TotalDiscount,
                                        StartDate = b.StartDate,
                                        EndDate = b.EndDate,
                                        RoomEventID = b.RoomEventID,
                                        Status = b.Status,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        Address = u.Address,
                                        City = u.City,
                                        State = u.State,
                                        Postcode = u.Postcode,
                                        Country = u.Country,
                                        Website = u.Website,
                                        MobileNo = u.MobileNo,
                                        Email = u.Email,
                                        RoomName = r.Description,
                                        HourlyPrice = r.HourlyPrice,
                                        DiscountPercentage = r.DiscountPercentage,
                                        Capacity = r.Capacity,
                                        TotalBeds = r.TotalBeds,
                                        TotalSofas = r.TotalSofas,
                                        RoomImagePath = r.RoomImages.Select(img => img.ImagePath).ToList(),
                                        RoomID = r.ID,
                                        RoomEventsName = re.Name,
                                    };

                var response = userId > 0
                    ? await bookingsQuery.Where(x => x.UserID == userId).ToListAsync().ConfigureAwait(false)
                    : await bookingsQuery.ToListAsync().ConfigureAwait(false);

                await tran.CommitAsync().ConfigureAwait(false);
                return response;

            }
            catch (Exception ex)
            {
                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return  new List<UserBookingResponse>();
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }
    }
}
