using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class User : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public int RoleID { get; set; }  // Roles: SuperAdmin, FranchiseeAdmin, Customer
        public int FranchiseeAdminID { get; set; }  // User For Franchisee Admin
        public bool IsDelete { get; set; }
        public Role Roles { get; set; }
    }
}
