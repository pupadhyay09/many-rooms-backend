using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Infrastructure.Helpers;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class RoomCreateUsecase : IRoomCreateUsecase
    {
        private readonly IRoomGateway _roomGateway;
        private readonly IRoomEventGateway _roomEventGateway;
        private readonly IRoomImageGateway _roomImageGateway;
        private readonly IRoomStaffMappingGateway _roomStaffMappingGateway;
        public RoomCreateUsecase(IRoomGateway roomGateway, IRoomEventGateway roomEventGateway, 
            IRoomImageGateway roomImageGateway, IRoomStaffMappingGateway roomStaffMappingGateway)
        {
            this._roomGateway = roomGateway;
            this._roomEventGateway = roomEventGateway;
            this._roomImageGateway = roomImageGateway;
            this._roomStaffMappingGateway = roomStaffMappingGateway;
        }
        public async Task<RoomResponse> ExecuteAsync(RoomRequest request)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            var response = new RoomResponse() { Iserror = false, ErrorMessage = "" };
            var roomeventitemlist = new List<RoomEvent>();
            var roomImageitemlist = new List<RoomImage>();
            var roomStaffitemlist = new List<RoomStaffMapping>();
            var additem = request.ToEntity();
            _roomGateway.SetDb(db);
            _roomEventGateway.SetDb(db);
            _roomImageGateway.SetDb(db);
            try
            {
                var addroomitem = await _roomGateway.Add(additem).ConfigureAwait(false);
                //await db.SaveChangesAsync().ConfigureAwait(false);
                if (addroomitem == null)
                    return new RoomResponse { Iserror = true, ErrorMessage = "room not create" };

                if (request.RoomEventID.Count > 0)
                {
                    foreach (var item in request.RoomEventID)
                    {
                        roomeventitemlist.Add(new RoomEvent()
                        {
                            RoomID = addroomitem.ID,
                            EventID = item,
                            IsDelete = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = 1,
                            UpdatedBy = 1,
                            UpdatedAt = DateTime.UtcNow,
                        });
                    }
                    if (roomeventitemlist.Count > 0)
                        await _roomEventGateway.AddRange(roomeventitemlist).ConfigureAwait(false);
                }

                if (request.File != null && request.File.Count() > 0)
                {
                    foreach (var item in request.File)
                    {
                        var fileurl = await FileUpload.ExecuteFilAsync(item).ConfigureAwait(false);
                        if (!string.IsNullOrEmpty(fileurl))
                        {
                            roomImageitemlist.Add(new RoomImage()
                            {
                                RoomID = addroomitem.ID,
                                DisplayOrder = 1,
                                ImagePath = fileurl,
                                Description = "",
                                IsDelete = false,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 1,
                                FileExtension = Path.GetExtension(item.FileName),
                                UpdatedAt= DateTime.UtcNow,
                                UpdatedBy   = 1,
                            });
                        }
                    }
                    if (roomImageitemlist.Count > 0)
                        await _roomImageGateway.AddRange(roomImageitemlist).ConfigureAwait(false);
                }

                if (request.RoomStaffID != null && request.RoomStaffID.Count() > 0)
                {
                    foreach (var item in request.RoomStaffID)
                    {   
                        if (item > 0)
                        {
                            roomStaffitemlist.Add(new RoomStaffMapping()
                            {
                                RoomID = addroomitem.ID,
                                UserID  = item,
                                IsDelete = false,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = 1,
                                UpdatedAt = DateTime.UtcNow,
                                UpdatedBy = 1,
                            });
                        }
                    }
                    if (roomStaffitemlist.Count > 0)
                        await _roomStaffMappingGateway.AddRange(roomStaffitemlist).ConfigureAwait(false);
                }
                
                await tran.CommitAsync().ConfigureAwait(false);
                response = addroomitem.ToResponse();
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return new RoomResponse { Iserror = true, ErrorMessage = ex.Message };
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
            return response;
        }
    }
}
