using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Infrastructure.Exceptions;
using ManyRoomStudio.UseCases;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers.Api
{

    //[Authorize(Roles = $"{nameof(ERole.FranchiseeAdmin)},{nameof(ERole.Customer)}")]
    [EnableCors("AllowOrigin")]
    [Route("api/v1/rooms")]
    [ApiController]
    [Produces("application/json")]
    public class RoomsApiController : ControllerBase
    {
        private readonly IRoomGateway _roomGateway;
        private readonly IRoomSearchUsecase _roomSearchUsecase;
        private readonly IRoomCreateUsecase _roomCreateUsecase;
       public RoomsApiController(IRoomGateway roomGateway , IRoomSearchUsecase roomSearchUsecase , IRoomCreateUsecase roomCreateUsecase)
        {
            this._roomGateway = roomGateway;
            this._roomSearchUsecase = roomSearchUsecase;
            this._roomCreateUsecase = roomCreateUsecase;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var roomitem = await _roomGateway.Get(x => x.ID == id && x.IsDelete == false).ConfigureAwait(false);
            if (roomitem == null)
                return NotFound();


            return Ok(roomitem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }

           var  roomitecreate = await _roomCreateUsecase.ExecuteAsync(request).ConfigureAwait(false);
            if (roomitecreate == null)
                return NotFound();

            return Ok(roomitecreate);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoomRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }
            var roomitem = request.ToEntity();
            //var roomitemupdate = await _roomGateway.Update(roomitem).ConfigureAwait(false);
            //if (roomitemupdate == null)
            //    return NotFound();

            //return Ok(roomitemupdate);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var roomitem = await _roomGateway.Get(x => x.ID == id && x.IsDelete == false).ConfigureAwait(false);
            if (roomitem == null)
                return NotFound();

            return Ok(roomitem);
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetAllRoomByFilter(RoomSearchRequest request)
        {
           var roomlist =  await _roomSearchUsecase.ExecuteAsync(request).ConfigureAwait(false);
            return Ok(roomlist);
        }
    }
}
