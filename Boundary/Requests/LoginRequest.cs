using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Boundary.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = "admin@admin.com";
        [Required]
        public string Password { get; set; } = "Eglaf@1234";
        public string ErrorMessage { get; set; }
    }
}
