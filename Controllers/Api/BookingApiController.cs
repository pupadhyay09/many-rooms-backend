using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Infrastructure.Exceptions;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers.Api
{
    [EnableCors("AllowOrigin")]
    [Route("api/v1/booking")]
    [ApiController]
    [Produces("application/json")]
    public class BookingApiController : ControllerBase
    {
        private readonly IBookingGateway _bookingGateway;
        private readonly IRoomGateway _roomGateway;
        private readonly IStudioBookUsecase _studioBookUsecase;
        private readonly IUserBookingUsecase _userBookingUsecase;
        public BookingApiController(IBookingGateway bookingGateway , 
                                IRoomGateway roomGateway, 
                                IStudioBookUsecase studioBookUsecase,
                                IUserBookingUsecase userBookingUsecase)
        {
            this._bookingGateway = bookingGateway;
            this._roomGateway = roomGateway;
            this._studioBookUsecase = studioBookUsecase;
            this._userBookingUsecase = userBookingUsecase;

        }


        [HttpPost("studiobooking")]
        public async Task<IActionResult> BookStudio(BookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }
            var roombookingcreate = await _studioBookUsecase.ExecuteAsync(request).ConfigureAwait(false);
            if (roombookingcreate != null && roombookingcreate.Iserror == false)
                 return Ok(roombookingcreate);
            else if(roombookingcreate != null && roombookingcreate.Iserror == true)
                return NotFound(roombookingcreate.ErrorMessage);

            return NotFound();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetStudioBookingsByUserId(int userId)
        {
             var bookinglist = await _userBookingUsecase.ExecuteAsync(userId).ConfigureAwait(false);
            return Ok(bookinglist);
        }
    }
}
