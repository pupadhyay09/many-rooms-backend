using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.RazorPartial;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers.Api
{
    [EnableCors("AllowOrigin")]
    [Route("api/v1/roomstaffmapping")]
    [ApiController]
    [Produces("application/json")]
    public class RoomStaffMappingApiController : ControllerBase
    {
        
        private readonly IStaffRoomMappingByUserIdUsecase  _staffRoomMappingByUserIdUsecase;
        private readonly IRazorPartialToString _partial;
        private readonly IRoomStaffMappingGateway  _roomStaffMappingGateway;

        public RoomStaffMappingApiController(IStaffRoomMappingByUserIdUsecase staffRoomMappingByUserIdUsecase ,
               IRazorPartialToString partial , IRoomStaffMappingGateway roomStaffMappingGateway) {
            this._staffRoomMappingByUserIdUsecase = staffRoomMappingByUserIdUsecase;
            this._partial = partial;
            this._roomStaffMappingGateway = roomStaffMappingGateway;
        }


        [HttpGet("staff/room/mapping/{staffid}")]
        public async Task<IActionResult> GetStaffRoomMappingByUserId(int staffid)
        {
            var body = string.Empty;
            var allroom = await _staffRoomMappingByUserIdUsecase.ExecuteAsync(staffid).ConfigureAwait(false);


            if (allroom != null && allroom.Any())
                body = await _partial.Render("~/Views/Staff/_staffroom.cshtml", allroom).ConfigureAwait(false);

            return Ok(body);

        }

        [HttpPost("staff/assignedroom")]
        public async Task<IActionResult> AssignedRoomIds(AssignedRoomReqest reqest)
        {
            try
            {
                var retval = false;
                var allroomMapping = await _roomStaffMappingGateway.GetAll(x => x.UserID == reqest.UserId && x.IsDelete == false).ConfigureAwait(false);
                if (reqest?.RoomIds != null && reqest?.RoomIds?.Count > 0)
                {
                    var existingroomIds = allroomMapping?.Select(x => x.RoomID).ToHashSet() ?? new HashSet<int>();

                    var newroomAssigned = reqest.RoomIds
                                        .Where(x => !existingroomIds.Contains(x))
                                        .Select(r => new RoomStaffMapping
                                        {
                                            RoomID = r,
                                            UserID = reqest.UserId,
                                            IsDelete = false
                                        }).ToList();
                    var toDelete = allroomMapping?
                                      .Where(item => !reqest.RoomIds.Contains(item.RoomID))
                                      .ToList();
                    if (toDelete?.Count > 0)
                        await _roomStaffMappingGateway.RemoveRange(toDelete).ConfigureAwait(false);

                    if (newroomAssigned.Count > 0)
                        await _roomStaffMappingGateway.UpdateRange(newroomAssigned).ConfigureAwait(false);

                    retval = true;
                }
                else
                {
                    if (allroomMapping != null && allroomMapping.Count() > 0)
                        await _roomStaffMappingGateway.RemoveRange(allroomMapping.ToList()).ConfigureAwait(false);

                    retval = true;
                }
                return Ok(retval);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
