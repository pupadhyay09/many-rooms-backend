using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Infrastructure.Exceptions;
using ManyRoomStudio.Infrastructure.Helpers;
using ManyRoomStudio.Infrastructure.RazorPartial;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace ManyRoomStudio.Controllers.Api
{
    [Authorize]
    [EnableCors("AllowOrigin")]
    [Route("api/v1/user")]
    [ApiController]
    [Produces("application/json")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserGateway _userGatway;
        private readonly IvwStaffModelGateway _vwStaffModelGateway;
        private readonly IFranchiseekeyGateway _firanchiseekeyGateway;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRazorPartialToString _partial;
        private readonly IFranchiseeByIdUsecase _franchiseeByIdUsecase;
        
        public UserApiController(IUserGateway userGatway,
                        IFranchiseekeyGateway firanchiseekeyGateway,
                        IHttpContextAccessor contextAccessor,
                        IRazorPartialToString partial,
                        IvwStaffModelGateway vwStaffModelGateway,
                        IFranchiseeByIdUsecase franchiseeByIdUsecase ,
                        IRoomStaffMappingGateway roomStaffMappingGateway)
        {
            this._userGatway = userGatway;
            this._firanchiseekeyGateway = firanchiseekeyGateway;
            this._contextAccessor = contextAccessor;
            this._partial = partial;
            this._franchiseeByIdUsecase = franchiseeByIdUsecase;
            this._vwStaffModelGateway = vwStaffModelGateway;
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var userLoginResponse = new UserLoginResponse();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }

            var Saltkey = SecurityHelper.GenerateSalt();

            var userDetails = await _userGatway.Get(m => m.Email == request.Email && m.IsDelete == false).ConfigureAwait(false);
            if (userDetails == null)
                return NotFound("Sorry! User details are not found.");

            var Password = SecurityHelper.HashPassword(request.Password, userDetails.Salt);
            if (Password != userDetails.Password)
                return NotFound("Please enter valid password.");

            var user = userDetails.ToResponse();
            user.Token = TokenGenerator.GenerateToken(userDetails.ID.ToString(), userDetails.Email, user?.RolesName?.ToString());
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var useritem = await _userGatway.Get(x => x.ID == id && x.IsDelete == false).ConfigureAwait(false);
            if (useritem == null)
                return NotFound();

            return Ok(useritem);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllUserByFilter(string rolename)
        {
            int roleId = (int)Enum.Parse(typeof(ERole), rolename, true);
            var useritem = await _userGatway.GetAll(x => x.RoleID == roleId && x.IsDelete == false).ConfigureAwait(false);
            if (useritem == null)
                return NotFound();

            return Ok(useritem);
        }

        [HttpPost("createstaffuser")]
        public async Task<IActionResult> CreateStaffUserAsync(CreateStaffUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }
            var olduser = await _userGatway.Get(x => x.Email.ToLower() == request.Email.ToLower() && x.IsDelete == false).ConfigureAwait(false);
            if (olduser != null)
                return NotFound("User already exists");

            var staffUser = request.ToEntity();
            if (staffUser == null)
                return NotFound();
            var Saltkey = SecurityHelper.GenerateSalt();
            var Password = SecurityHelper.HashPassword(request.Password, Saltkey);
            staffUser.Salt = Saltkey;
            staffUser.RoleID = (int)ERole.Staff;
            staffUser.Password = Password;
            var staffcreate = await _userGatway.Add(staffUser).ConfigureAwait(false);
            if (staffcreate == null)
                return NotFound();

            return Ok(staffcreate);
        }

        [HttpPut("updatestaffuser")]
        public async Task<IActionResult> UpdateStaffUserAsync(UpdateStaffUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }
            var staffUser = await _userGatway.Get(x => x.ID == request.ID && x.IsDelete == false).ConfigureAwait(false);
            if (staffUser == null)
                return NotFound("User not found");

            var existingUser = await _userGatway.Get(x => x.ID != request.ID && x.Email.ToLower() == request.Email.ToLower()).ConfigureAwait(false);
            if (existingUser != null)
                return Conflict("A user with this email already exists.");

            staffUser.FirstName = request.FirstName;
            staffUser.LastName = request.LastName;
            staffUser.Address = request.Address;
            staffUser.City = request.City;
            staffUser.State = request.State;
            staffUser.Postcode = request.Postcode;
            staffUser.Country = request.Country;
            staffUser.Website = request.Website;
            staffUser.MobileNo = request.MobileNo;
            staffUser.Email = request.Email;
            if (staffUser.FranchiseeAdminID == 0)
                staffUser.FranchiseeAdminID = request.FranchiseeAdminID;

            var staffeupdate = await _userGatway.Update(staffUser).ConfigureAwait(false);
            if (staffeupdate == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user.");

            return Ok(staffeupdate);
        }

        [HttpDelete("staff/delete/{id}")]
        public async Task<IActionResult> DeleteStaffAsync(int id)
        {
            int userid = HttpContext.Session.GetInt32("UserId") ?? 0;

            var retval = false;
            var useritem = await _userGatway.Get(x => x.ID == id && x.IsDelete == false).ConfigureAwait(false);
            if (useritem == null)
                return NotFound();

            useritem.IsDelete = true;
            useritem.UpdatedBy = userid;
            var deleteuser = await _userGatway.Update(useritem).ConfigureAwait(false);
            if (deleteuser != null)
                retval = true;

            return Ok(retval);
        }

        [HttpPost("createfranchiseeuser")]
        public async Task<IActionResult> CreateFranchiseeUserAsync(CreateFranchiseeUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }
            var olduser = await _userGatway.Get(x => x.Email.ToLower() == request.Email.ToLower() && x.IsDelete == false).ConfigureAwait(false);
            if (olduser != null)
                return NotFound("User already exists");

            var franchiseeUser = request.ToEntity();
            if (franchiseeUser == null)
                return NotFound();
            var Saltkey = SecurityHelper.GenerateSalt();
            var Password = SecurityHelper.HashPassword(request.Password, Saltkey);
            franchiseeUser.Salt = Saltkey;
            franchiseeUser.RoleID = (int)ERole.FranchiseeAdmin;
            franchiseeUser.Password = Password;
            var franchiseeUsercreate = await _userGatway.Add(franchiseeUser).ConfigureAwait(false);
            if (franchiseeUsercreate == null)
                return NotFound();
            var keyadd = new Franchiseekey()
            {
                UserID = franchiseeUsercreate.ID,
                Publishablekey = request.Publishablekey,
                Secretkey = request.Secretkey,
                Paymnetkeytype = EPaymnetType.Stripe.ToString()
            };
            await _firanchiseekeyGateway.Add(keyadd).ConfigureAwait(false);

            return Ok(franchiseeUsercreate);
        }

        [HttpPut("updatefranchiseeuser")]
        public async Task<IActionResult> UpdateFranchiseeUserAsync(UpdateFranchiseeUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorMessages();
                return BadRequest(errors);
            }

            var franchiseeUser = await _userGatway.Get(x => x.ID == request.ID).ConfigureAwait(false);
            if (franchiseeUser == null)
                return NotFound("User not found");

            var existingUser = await _userGatway.Get(x => x.ID != request.ID && x.Email.ToLower() == request.Email.ToLower() && x.IsDelete == false).ConfigureAwait(false);
            if (existingUser != null)
                return Conflict("A user with this email already exists.");

            franchiseeUser.FirstName = request.FirstName;
            franchiseeUser.LastName = request.LastName;
            franchiseeUser.Address = request.Address;
            franchiseeUser.City = request.City;
            franchiseeUser.State = request.State;
            franchiseeUser.Postcode = request.Postcode;
            franchiseeUser.Country = request.Country;
            franchiseeUser.Website = request.Website;
            franchiseeUser.MobileNo = request.MobileNo;
            franchiseeUser.Email = request.Email;

            var roomiteupdate = await _userGatway.Update(franchiseeUser).ConfigureAwait(false);
            if (roomiteupdate == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update user.");

            var keyupdate = await _firanchiseekeyGateway.Get(x => x.ID == request.FranchiseekeyId).ConfigureAwait(false);
            if (keyupdate != null)
            {
                keyupdate.Publishablekey = request.Publishablekey;
                keyupdate.Secretkey = request.Secretkey;
                await _firanchiseekeyGateway.Update(keyupdate).ConfigureAwait(false);
            }
            else
            {
                var keyadd = new Franchiseekey()
                {
                    UserID = franchiseeUser.ID,
                    Publishablekey = request.Publishablekey,
                    Secretkey = request.Secretkey,
                };
                await _firanchiseekeyGateway.Add(keyadd).ConfigureAwait(false);
            }

            return Ok(roomiteupdate);
        }

        [HttpGet("staff/franchisee/{franchiseeUserId}")]
        public async Task<IActionResult> GetStaffByFranchisee(int franchiseeUserId)
        {

            var staffUsers = await _userGatway.GetAll(x =>
                    x.FranchiseeAdminID == franchiseeUserId &&
                    !x.IsDelete &&
                    x.RoleID == (int)ERole.Staff
                ).ConfigureAwait(false);

            if (staffUsers == null || !staffUsers.Any())
                return NotFound();

            return Ok(staffUsers);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            int userid = HttpContext.Session.GetInt32("UserId") ?? 0;

            var retval = false;
            var useritem = await _userGatway.Get(x => x.ID == id && x.IsDelete == false).ConfigureAwait(false);
            if (useritem == null)
                return NotFound();

            useritem.IsDelete = true;
            useritem.UpdatedBy = userid;
            var deleteuser = await _userGatway.Update(useritem).ConfigureAwait(false);
            if (deleteuser != null)
                retval = true;

            return Ok(retval);
        }

        [HttpGet("franchisee/user/{id}")]
        public async Task<IActionResult> GetFranchiseeUserAsync(int id)
        {
            var franchiseeUser = await _franchiseeByIdUsecase.ExecuteAsync(id).ConfigureAwait(false);
            if (franchiseeUser == null)
                return NotFound(new { message = "Franchisee user not found" });

            return Ok(franchiseeUser);
        }


        [HttpGet("franchisee")]
        public async Task<IActionResult> OnLoadFranchiseeList()
        {
            var users = new List<UserModelResponse>();

            var franchiseeuser = await _userGatway.GetAll(x => x.IsDelete == false && x.RoleID == (int)ERole.FranchiseeAdmin).ConfigureAwait(false);
            if (franchiseeuser != null && franchiseeuser.Any())
                users = franchiseeuser.OrderByDescending(x => x.UpdatedAt).ToList().ToUserResponse();
            var body = await _partial.Render("~/Views/Franchisee/_franchiseelist.cshtml", users).ConfigureAwait(false);
            return Ok(body);
        }

        [HttpGet("onloadstaffList")]
        public async Task<IActionResult> OnLoadStaffList(int franchiseeUserID)
        {
            var users = new List<StaffModelResponse>();

            var staffuser = await _vwStaffModelGateway.GetAll(x => x.IsDelete == false).ConfigureAwait(false);
            if (staffuser != null && staffuser.Any())
            {
                if (franchiseeUserID > 0)
                    staffuser = staffuser.Where(x => x.FranchiseeId == franchiseeUserID).ToList();

                users = staffuser.OrderByDescending(x => x.UpdatedAt).ToList().TosttaffResponse();
            }
            var body = await _partial.Render("~/Views/Staff/_stafflist.cshtml", users).ConfigureAwait(false);
            return Ok(body);
        }

    }
}
