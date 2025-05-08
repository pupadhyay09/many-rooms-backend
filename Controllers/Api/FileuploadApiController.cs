using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Infrastructure.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers.Api
{
    [Authorize(Roles = $"{nameof(ERole.FranchiseeAdmin)},{nameof(ERole.Customer)}")]
    [Route("api/v1/fileupload")]
    [ApiController]
    [Produces("application/json")]
    public class FileuploadApiController : ControllerBase
    {
        //private readonly ICreateFileuploadUseCase _createFileuploadUseCase;
        //public FileuploadApiController(IHttpContextAccessor contextAccessor,
        //    ICreateFileuploadUseCase createFileuploadUseCase) : base(contextAccessor)
        //{
        //    this._createFileuploadUseCase = createFileuploadUseCase;
        //    this._fileuploadGateway = fileuploadGateway;
        //}

        //[HttpPost("create/fileupload")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> Createfileupload([FromForm] FileuploadRequest request)
        //{
        //    var response = await _createFileuploadUseCase.ExecuteAsync(request).ConfigureAwait(false);
        //    return Ok(response);

        //}
    }
}
