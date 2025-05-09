using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Boundary.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}
