using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure;
using ManyRoomStudio.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;


namespace ManyRoomStudio.Controllers
{
    public class FranchiseeController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserGateway _userGatway;
        public FranchiseeController(IUserGateway userGatway, IHttpContextAccessor contextAccessor)
        {   
            this._contextAccessor = contextAccessor;
            this._userGatway = userGatway;
        }
        public async Task<IActionResult> Index()
        {
            var users = new List<UserModelResponse>();
            var userRole = _contextAccessor.HttpContext.Session.GetString("UserRole");

            bool isInvalidRole = string.IsNullOrEmpty(userRole) ||
                     userRole == ERole.Staff.ToString() ||
                     userRole == ERole.Manager.ToString() ||
                     userRole == ERole.Customer.ToString() ||
                     userRole == ERole.FranchiseeAdmin.ToString();

            if (isInvalidRole)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            var franchiseeuser = await _userGatway.GetAll(x => x.IsDelete == false && x.RoleID == (int)ERole.FranchiseeAdmin).ConfigureAwait(false);
            if (franchiseeuser != null && franchiseeuser.Any())
                users = franchiseeuser.OrderByDescending(x=>x.UpdatedAt).ToList().ToUserResponse();

            return View(users);
        }
    }
}
