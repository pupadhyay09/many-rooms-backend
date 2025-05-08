using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManyRoomStudio.UseCases
{
    public class FranchiseeByIdUsecase : IFranchiseeByIdUsecase
    {
        private readonly IFranchiseekeyGateway _firanchiseekeyGateway;
        public FranchiseeByIdUsecase(IFranchiseekeyGateway franchiseekeyGateway) {
            this._firanchiseekeyGateway = franchiseekeyGateway;
        }
        public async Task<FranchiseeUserModelResponse> ExecuteAsync(int Id)
        {
            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {   

                var retval =await (from u in db.Users.AsNoTracking().Include(x=>x.Roles)
                             join f in db.Franchiseekeys.AsNoTracking()
                             on u.ID equals f.UserID into franchiseeKeys
                             from f in franchiseeKeys.DefaultIfEmpty()
                             where u.IsDelete == false && u.ID == Id &&
                             (f == null ||  (f.Paymnetkeytype == EPaymnetType.Stripe.ToString() && f.IsDelete == false))
                             select new FranchiseeUserModelResponse
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
                                 RoleID = u.RoleID,
                                 RolesName = u.Roles.RoleName,
                                 Publishablekey = f != null ? f.Publishablekey :"",
                                 Secretkey = f != null ? f.Secretkey : "",
                                 FranchiseekeyId = f != null ? f.ID : 0,
                                 Password = u.Password,
                             }).FirstOrDefaultAsync();

                await tran.CommitAsync().ConfigureAwait(false);
                return retval;
            }
            catch (Exception ex)
            {

                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return new  FranchiseeUserModelResponse();
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }
    }
}
