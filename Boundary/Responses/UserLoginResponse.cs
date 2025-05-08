namespace ManyRoomStudio.Boundary.Responses
{
    public class UserLoginResponse
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
        public string? Email { get; set; }
        public int RoleID { get; set; }
        public int FranchiseeAdminID { get; set; }
        public bool IsDelete { get; set; }
        public string? RolesName { get; set; }
        public string Token { get; set; }
    }
}
