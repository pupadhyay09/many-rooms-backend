using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Boundary.Requests
{
    public class CreateFranchiseeUserRequest
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Postcode { get; set; }
        public string? Country { get; set; }
        public string? Website { get; set; }
        public string? MobileNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string? Publishablekey { get; set; }
        public string? Secretkey { get; set; }
    }
}
