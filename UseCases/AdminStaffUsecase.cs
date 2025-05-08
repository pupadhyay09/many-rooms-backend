using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Domains.Rooms;
using ManyRoomStudio.Domains.Staff;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ManyRoomStudio.UseCases
{
    public class AdminStaffUsecase : IAdminStaffUsecase
    {
        private readonly IUserGateway _userGatway;
        public AdminStaffUsecase(IUserGateway userGatway)
        {
            this._userGatway = userGatway;
        }
        public async Task<AdminStaffDomain> ExecuteAsync(int FranchiseeUserID)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);
            var retval = new AdminStaffDomain();
            _userGatway.SetDb(db);
            try
            {

                var staffuser = await (from u in db.Users.AsNoTracking().Include(x => x.Roles)
                                       join f in db.Users.AsNoTracking()
                                       on u.FranchiseeAdminID equals f.ID into users
                                       from f in users.DefaultIfEmpty()
                                       where u.IsDelete == false && u.RoleID == (int)ERole.Staff && u.FranchiseeAdminID > 0
                                       select new StaffModelResponse
                                       {
                                           ID = u.ID,
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
                                           RolesName = u.Roles.RoleName,
                                           Password = u.Password,
                                           FranchiseeName = f.FirstName + " " + f.LastName,
                                           FranchiseeEmail = f.Email,
                                           FranchiseeId = f.FranchiseeAdminID,
                                       }).ToListAsync();
                if (FranchiseeUserID == 0)
                {
                    var franchiseeuser = await _userGatway.GetAll(x => x.RoleID == (int)ERole.FranchiseeAdmin && x.IsDelete == false).ConfigureAwait(false);

                    if (franchiseeuser != null && franchiseeuser.Any())
                    {
                        var response = franchiseeuser.Select(x => new RoomFranchiseeUserResponse
                        {
                            Value = x.ID.ToString(),
                            Text = x.FirstName + " " + x.LastName
                        }).ToList();

                        retval.franchiseeUserResponses = response;
                    }
                }
                if (staffuser != null && staffuser.Any())
                {
                    if (FranchiseeUserID > 0)
                        retval.staffuserResponses = staffuser.Where(x => x.FranchiseeId == FranchiseeUserID).ToList();
                    else
                        retval.staffuserResponses = staffuser.ToList();
                }

                await tran.CommitAsync().ConfigureAwait(false);
                return retval;
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
