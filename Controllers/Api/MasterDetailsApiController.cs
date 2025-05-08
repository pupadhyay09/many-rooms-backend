using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers.Api
{
    [EnableCors("AllowOrigin")]
    [Route("api/v1/masterDetails")]
    [ApiController]
    [Produces("application/json")]
    public class MasterDetailsApiController : ControllerBase
    {
        private readonly IMasterDetailsGateway  _masterDetailsGateway;

        public MasterDetailsApiController(IMasterDetailsGateway masterDetailsGateway) { 
            this._masterDetailsGateway = masterDetailsGateway;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var masterDetailsitem = await _masterDetailsGateway.Get(x => x.ID == id && x.IsDelete == false).ConfigureAwait(false);
            if (masterDetailsitem == null)
                return NotFound();

            return Ok(masterDetailsitem);
        }

        [HttpGet("getitemsbycategory")]
        public async Task<IActionResult> GetItemsByCategory(string Category)
        {

            var masterDetailsitem = await _masterDetailsGateway.GetAll(x => x.Category.Trim().ToLower() == Category.Trim().ToLower() && x.IsDelete == false).ConfigureAwait(false);
            if (masterDetailsitem == null)
                return NotFound();

            return Ok(masterDetailsitem.OrderByDescending(x => x.Name).ToList().ToResponse());
        }
    }
}
