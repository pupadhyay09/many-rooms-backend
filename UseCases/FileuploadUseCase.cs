using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Helpers;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.Repository;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ManyRoomStudio.UseCases
{
    public class FileuploadUsecase : IFileuploadUsecase
    {
        private readonly IRoomGateway _roomGateway;
        private readonly IRoomImageGateway _roomImageGateway;
        public FileuploadUsecase(IRoomGateway roomGateway, IRoomImageGateway roomImageGateway)
        {
            this._roomGateway = roomGateway;
            this._roomImageGateway = roomImageGateway;
        }
        public async Task<FileuploadResponse> ExecuteAsync(FileuploadRequest request)
        {

            await using var db = ManyRoomStudioContext.Create();
            await using var tran = await db.Database.BeginTransactionAsync().ConfigureAwait(false);
            var response = new FileuploadResponse() { Iserror = false, ErrorMessage = "" };
            var Fileuploadlist = new List<RoomImage>();
            _roomImageGateway.SetDb(db);
            if (request.TargetId == 0)
                return new FileuploadResponse { Iserror = true, ErrorMessage = "Target Type is mandatory." };

            try
            {
               
                if (request.File != null && request.File.Count() > 0)
                {  
                    foreach (var item in request.File)
                    {
                        response.TotalFile = response.TotalFile + 1;
                        var fileurl = await FileUpload.ExecuteFilAsync(item).ConfigureAwait(false);
                        if (!string.IsNullOrEmpty(fileurl))
                        {
                            Fileuploadlist.Add(new RoomImage()
                            {

                                ImagePath = fileurl,
                                RoomID = request.TargetId,
                                FileExtension = Path.GetExtension(item.FileName),
                                DisplayOrder = 1,
                                IsDelete = false,
                                Description = "",
                                CatalogUrl = "",
                            });
                            response.TotalSuccessful = response.TotalSuccessful + 1;
                        }
                        else
                            response.TotalErrorFile = response.TotalErrorFile + 1;
                    }
                }
                if (Fileuploadlist.Count > 0)
                    await _roomImageGateway.AddRange(Fileuploadlist).ConfigureAwait(false);

                await tran.CommitAsync().ConfigureAwait(false);
                return response;
            }
            catch (Exception ex)
            {

                await tran.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
                return new FileuploadResponse { Iserror = true, ErrorMessage = ex.Message };
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }
    }
}
