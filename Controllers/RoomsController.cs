using ManyRoomStudio.Boundary.Responses;
using ManyRoomStudio.Domains.Rooms;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.Models.Entities;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ManyRoomStudio.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomManagementUsecase _roomManagementUsecase;
        private readonly IHttpContextAccessor _contextAccessor;
        public RoomsController(IRoomManagementUsecase roomManagementUsecase, IHttpContextAccessor contextAccessor)
        {
            this._roomManagementUsecase = roomManagementUsecase;
            this._contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index()
        {  
            var userRole = _contextAccessor.HttpContext.Session.GetString("UserRole");

            bool isInvalidRole = string.IsNullOrEmpty(userRole) ||
                     userRole == ERole.Staff.ToString() ||
                     userRole == ERole.Manager.ToString() ||
                     userRole == ERole.FranchiseeAdmin.ToString() ||
                     userRole == ERole.Customer.ToString();

            if (isInvalidRole)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            var roomManagementDomain = await _roomManagementUsecase.ExecuteAsync().ConfigureAwait(false);
            return View(roomManagementDomain);
        }

    }
}
