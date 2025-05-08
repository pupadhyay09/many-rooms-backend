using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Boundary.Requests
{
    public class FranchiseeAdminRequest
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }
        [Required(ErrorMessage = "Postcode is required.")]
        public string Postcode { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
        public string? Website { get; set; }
        [Required(ErrorMessage = "Mobile No is required.")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public int LoginUserId { get; set; }
    }
}
