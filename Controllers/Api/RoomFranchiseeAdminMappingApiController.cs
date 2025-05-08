using Azure.Core;
using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Infrastructure.RazorPartial;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers.Api
{
    [EnableCors("AllowOrigin")]
    [Route("api/v1/roomfranchiseeadminmapping")]
    [ApiController]
    [Produces("application/json")]
    public class RoomFranchiseeAdminMappingApiController : ControllerBase
    {
        private readonly IRoomFranchiseeAdminMappingGateway _roomFranchiseeAdminMappingGateway;
        private readonly IFranchiseeRoomMappingByUserIdUsecase _franchiseeRoomMappingByUserIdUsecase;
        private readonly IRazorPartialToString _partial;
        public RoomFranchiseeAdminMappingApiController(IRoomFranchiseeAdminMappingGateway roomFranchiseeAdminMappingGateway,
                                IRoomGateway roomGateway,
                                IRazorPartialToString partial,
                                IFranchiseeRoomMappingByUserIdUsecase franchiseeRoomMappingByUserIdUsecase)
        {
            
            this._roomFranchiseeAdminMappingGateway = roomFranchiseeAdminMappingGateway;
            this._partial = partial;
            this._franchiseeRoomMappingByUserIdUsecase = franchiseeRoomMappingByUserIdUsecase;
        }


        [HttpGet("franchisee/room/mapping/{userId}")]
        public async Task<IActionResult> GetFranchiseeRoomMappingByUserId(int userId)
        {
            var allroom = await _franchiseeRoomMappingByUserIdUsecase.ExecuteAsync(userId).ConfigureAwait(false);

            var body = string.Empty;
            if (allroom != null && allroom.Any())
                body = await _partial.Render("~/Views/Franchisee/_roomfranchisee.cshtml", allroom).ConfigureAwait(false);

            return Ok(body);

        }

        [HttpPost("user/assignedroom")]
        public async Task<IActionResult> AssignedRoomIds(AssignedRoomReqest reqest)
        {
            try
            {
                var retval = false;
                var allroomMapping = await _roomFranchiseeAdminMappingGateway.GetAll(x => x.UserID == reqest.UserId && x.IsDelete == false).ConfigureAwait(false);
                if (reqest?.RoomIds != null && reqest?.RoomIds?.Count > 0)
                {
                    var existingroomIds = allroomMapping?.Select(x => x.RoomID).ToHashSet() ?? new HashSet<int>();

                    var newroomAssigned = reqest.RoomIds
                                        .Where(x => !existingroomIds.Contains(x))
                                        .Select(r => new RoomFranchiseeAdminMapping
                                        {
                                            RoomID = r,
                                            UserID = reqest.UserId,
                                            IsDelete = false
                                        }).ToList();
                    var toDelete = allroomMapping?
                                      .Where(item => !reqest.RoomIds.Contains(item.RoomID))
                                      .ToList();
                    if (toDelete?.Count > 0)
                        await _roomFranchiseeAdminMappingGateway.RemoveRange(toDelete).ConfigureAwait(false);

                    if (newroomAssigned.Count > 0)
                        await _roomFranchiseeAdminMappingGateway.UpdateRange(newroomAssigned).ConfigureAwait(false);
                    
                    retval = true;
                }
                else
                {
                    if (allroomMapping != null && allroomMapping.Count() > 0)
                        await _roomFranchiseeAdminMappingGateway.RemoveRange(allroomMapping.ToList()).ConfigureAwait(false);

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
