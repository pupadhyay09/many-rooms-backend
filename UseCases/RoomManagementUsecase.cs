using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Domains.Rooms;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ManyRoomStudio.UseCases
{
    public class RoomManagementUsecase : IRoomManagementUsecase
    {
        private readonly IRoomGateway _roomGateway;
        private readonly IUserGateway _userGatway;
        private readonly IMasterDetailsGateway _masterDetailsGateway;

        public RoomManagementUsecase(IRoomGateway roomGateway, IUserGateway userGatway, IMasterDetailsGateway masterDetailsGateway)
        {
            this._roomGateway = roomGateway;
            this._userGatway = userGatway;
            this._masterDetailsGateway = masterDetailsGateway;
        }

        public async Task<RoomManagementDomain> ExecuteAsync()
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            var roomManagementDomain = new RoomManagementDomain();

            try
            {
                _roomGateway.SetDb(db);
                _userGatway.SetDb(db);
                _masterDetailsGateway.SetDb(db);

                var retval = db.Rooms.AsNoTracking()
                            .Include(x=>x.OwnershipType)
                            .Where(x => x.IsDelete == false && x.RoomEvents != null && x.RoomEvents.Any(e => e.IsDelete == false)).OrderByDescending(x => x.UpdatedAt)
                            .Select(x => new RoomManagementResponse
                            {
                                ID = x.ID,
                                Description = x.Description,
                                HourlyPrice = x.HourlyPrice,
                                DiscountPercentage = x.DiscountPercentage,
                                Capacity = x.Capacity,
                                TotalBeds = x.TotalBeds,
                                TotalSofas = x.TotalSofas,
                                RoomName = x.RoomName,
                                OwnershipTypeName = x.OwnershipType.Name,
                                IsVATEnabled = x.IsVATEnabled,
                                RoomEventsName = string.Join(", ", x.RoomEvents
                                        .Where(e => e.IsDelete == false)
                                        .Select(e => e.EventDetail.Name))
                            })
                            .ToList();

                var eventtype = await _masterDetailsGateway.GetAll(x => x.IsDelete == false && x.Category.ToLower() == ManyRoomStudioConstants.CategoryEvent.ToLower()).ConfigureAwait(false);

                if (eventtype != null && eventtype.Any())
                    roomManagementDomain.eventType = eventtype.ToList().ToResponse();

                var ownershipType = await _masterDetailsGateway.GetAll(x => x.IsDelete == false && x.Category.ToLower() == ManyRoomStudioConstants.CategoryOwnership.ToLower()).ConfigureAwait(false);

                if (ownershipType != null && ownershipType.Any())
                    roomManagementDomain.ownershipType = ownershipType.ToList().ToResponse();

                if (retval != null && retval.Any())
                    roomManagementDomain.roomManagementResponses = retval;

                await tran.CommitAsync().ConfigureAwait(false);
                return roomManagementDomain;
            }
            catch (Exception ex)
            {

                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return new RoomManagementDomain();
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }
    }
}
