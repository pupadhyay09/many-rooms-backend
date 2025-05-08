using ManyRoomStudio.Gateways.Interfaces;
using ManyRoomStudio.Infrastructure.Enums;
using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers
{
    public class StaffController : Controller
    {
        
        private readonly IAdminStaffUsecase _adminStaffUsecase;
        private readonly IHttpContextAccessor _contextAccessor;
        public StaffController(IHttpContextAccessor contextAccessor , IAdminStaffUsecase adminStaffUsecase )
        {   
            this._adminStaffUsecase = adminStaffUsecase;
            this._contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index()
        {   
            var userRole = _contextAccessor?.HttpContext?.Session.GetString("UserRole");

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
            var staffUser = await _adminStaffUsecase.ExecuteAsync(0).ConfigureAwait(false);
            return View(staffUser);
        }
    }
}
