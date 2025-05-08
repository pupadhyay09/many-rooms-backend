using ManyRoomStudio.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers
{
    public class BookController : Controller
    {
        private readonly IUserBookingUsecase _userBookingUsecase;
        private readonly IHttpContextAccessor _contextAccessor;
        public BookController(IHttpContextAccessor contextAccessor, IUserBookingUsecase userBookingUsecase)
        {
            this._userBookingUsecase = userBookingUsecase;
            this._contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userBooking = await _userBookingUsecase.ExecuteAsync(0).ConfigureAwait(false);
            return View(userBooking);
        }
    }
}
