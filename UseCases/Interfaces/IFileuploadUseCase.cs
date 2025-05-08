using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;

namespace ManyRoomStudio.UseCases.Interfaces
{
    public interface IFileuploadUsecase
    {
        Task<FileuploadResponse> ExecuteAsync(FileuploadRequest request);
    }
}
