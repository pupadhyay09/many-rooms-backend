namespace ManyRoomStudio.Boundary.Responses
{
    public class StaffModelResponse
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
        public string? RolesName { get; set; }
        public int FranchiseeId { get; set; }
        public string? FranchiseeName { get; set; }
        public string? FranchiseeEmail { get; set; }
        public string? Password { get; set; }
    }
}
