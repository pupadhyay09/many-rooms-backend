using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class FranchiseeRoomMappingByUserIdUsecase : IFranchiseeRoomMappingByUserIdUsecase
    {

        public async Task<List<FranchiseeRoomMappingResponse>> ExecuteAsync(int userId)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                var userRoomIds = await db.RoomFranchiseeAdminMappings
                          .Where(x => x.UserID == userId && !x.IsDelete)
                          .Select(x => x.RoomID)
                          .ToListAsync();

                var response = await db.Rooms
                            .Select(room => new FranchiseeRoomMappingResponse
                            {
                                RoomID = room.ID,
                                IsAssigned = userRoomIds.Contains(room.ID) ? true : false,
                                RoomName = room.RoomName,
                                UserID = userId,
                            })
                            .ToListAsync();

                await tran.CommitAsync().ConfigureAwait(false);
                return response;
            }
            catch (Exception ex)
            {

                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return null;
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }
    }
}
