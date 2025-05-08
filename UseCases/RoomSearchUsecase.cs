using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class RoomSearchUsecase : IRoomSearchUsecase
    {
        private readonly IRoomGateway _roomGateway;
        private readonly IRoomImageGateway _roomImageGateway;
        private readonly IBookingGateway _bookingGateway;

       public RoomSearchUsecase(IRoomGateway roomGateway, IRoomImageGateway roomImageGateway , IBookingGateway bookingGateway)
        {
            this._roomGateway = roomGateway;
            this._roomImageGateway = roomImageGateway;
            this._bookingGateway = bookingGateway;
        }
        public async Task<List<FilterRoomResponse>> ExecuteAsync(RoomSearchRequest request)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);
            var FilterRoomResponse = new List<FilterRoomResponse>();

            try
            {
                _roomGateway.SetDb(db);
                _roomImageGateway.SetDb(db);

                var retval = db.Rooms.AsNoTracking()
                                      .Where(x => x.IsDelete == false)
                                      .Include(x => x.RoomEvents)
                                                .Where(x => x.RoomEvents != null && x.RoomEvents.Any(e => e.IsDelete == false))
                                      .Include(x => x.RoomImages)
                                            .Where(x => x.RoomImages != null && x.RoomImages.Any(e => e.IsDelete == false))
                                      .OrderByDescending(x => x.UpdatedAt).AsQueryable();

                if (request.EventType != null && request.EventType > 0)
                    retval = retval.Where(x => x.RoomEvents != null && x.RoomEvents.Any(e => e.EventID == request.EventType));


                if (request.Numberofpeople != null && request.Numberofpeople > 0)
                    retval = retval.Where(x => x.Capacity >= request.Numberofpeople);

                if (request.BookingDate != null && request.BookingDate.HasValue)
                {

                    var allroomid = db.Bookings.AsNoTracking().
                                                Where(x=>x.IsDelete == false && 
                                                x.StartDate.HasValue && 
                                                x.StartDate.Value.Date == request.BookingDate.Value.Date).OrderByDescending(x => x.UpdatedAt).Select(x => x.RoomID).Distinct().ToList();
                    
                    if (allroomid != null && allroomid.Count > 0) 
                        retval = retval.Where(x=> !allroomid.Contains(x.ID));

                    if (request.StartTime != null && request.StartTime.HasValue)
                    {
                        var filterdate = request.BookingDate.Value.Date + request.StartTime.Value;
                        var allstratroomid = db.Bookings.AsNoTracking().
                                                   Where(x => x.IsDelete == false &&
                                                   x.StartDate.HasValue &&
                                                   x.StartDate.Value == filterdate).OrderByDescending(x => x.UpdatedAt).Select(x => x.RoomID).Distinct().ToList();
                        
                        if (allstratroomid != null && allstratroomid.Count > 0)
                            retval = retval.Where(x => !allstratroomid.Contains(x.ID));
                    }
                    if (request.EndTime != null && request.EndTime.HasValue)
                    {
                        var filterdate = request.BookingDate.Value.Date + request.EndTime.Value;
                        var allendroomid = db.Bookings.AsNoTracking().
                                                   Where(x => x.IsDelete == false &&
                                                   x.StartDate.HasValue &&
                                                   x.StartDate.Value == filterdate).OrderByDescending(x => x.UpdatedAt).Select(x => x.RoomID).Distinct().ToList();
                        if (allendroomid != null && allendroomid.Count > 0)
                            retval = retval.Where(x => !allendroomid.Contains(x.ID));
                    }
                }

                FilterRoomResponse = retval.Select(x => new FilterRoomResponse
                {
                    ID = x.ID,
                    Description = x.Description,
                    HourlyPrice = x.HourlyPrice,
                    DiscountAmount = x.DiscountAmount,
                    Capacity = x.Capacity,
                    TotalBeds = x.TotalBeds,
                    TotalSofas = x.TotalSofas,
                    RoomImagePath = x.RoomImages.Select(img => img.ImagePath).ToList(),
                    RoomEventsName = string.Join(", ", x.RoomEvents.Select(e => e.EventDetail.Name))
                }).ToList();
                await tran.CommitAsync().ConfigureAwait(false);
                return FilterRoomResponse;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return new List<FilterRoomResponse>();
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }
    }
}
