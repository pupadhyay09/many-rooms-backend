using Microsoft.AspNetCore.Mvc;

namespace ManyRoomStudio.Controllers
{
    public class ExceptionController : Controller
    {
        public IActionResult Index(string errorMessage)
        {
            throw new Exception("KLO Has fired a test exception!");
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
