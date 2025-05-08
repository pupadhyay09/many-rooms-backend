using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class StaffRoomMappingByUserIdUsecase : IStaffRoomMappingByUserIdUsecase
    {
        private readonly IUserGateway _userGatway;
       public StaffRoomMappingByUserIdUsecase(IUserGateway userGatway)
        {
            this._userGatway = userGatway;
        }
        public async Task<List<StaffRoomMappingResponse>> ExecuteAsync(int staffid)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                _userGatway.SetDb(db);

                var franchiseeAdminID = await db.Users.Where(x => x.ID == staffid).Select(x => x.FranchiseeAdminID).FirstOrDefaultAsync().ConfigureAwait(false);
                if(franchiseeAdminID == 0)
                    return new List<StaffRoomMappingResponse>();


                var userRoomIds = await db.RoomStaffMappings
                          .Where(x => x.UserID == staffid && !x.IsDelete)
                          .Select(x => x.RoomID)
                          .ToListAsync();

                 var response = await (from u in db.RoomFranchiseeAdminMappings.AsNoTracking()
                       join r in db.Rooms.AsNoTracking()
                       on u.RoomID equals r.ID
                       where u.UserID == franchiseeAdminID
                                       select new StaffRoomMappingResponse
                       {
                           RoomID = r.ID,
                           IsAssigned = userRoomIds.Contains(r.ID) ? true : false,
                           RoomName = r.RoomName != null  ? r.RoomName :"",
                           UserID = staffid,
                           
                       }).ToListAsync();

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
