using ManyRoomStudio.Boundary.Requests;
using ManyRoomStudio.Models.Entities;

namespace ManyRoomStudio.Models
{
    public class ViewModel
    {
        public ViewModel()
        {
        }

        public User SuperAdminDetail { get; set; }
        
        public User FranchiseeAdminDetail { get; set; }
        public List<User> FranchiseeAdminList { get; set; }
        public FranchiseeAdminRequest FranchiseeAdminRequest { get; set; }
    }
}
