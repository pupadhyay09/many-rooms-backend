using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManyRoomStudio.Domains.ClientDomains
{
    public class ClientsManagementDomain
    {
        public ClientsManagementDomain()
        {

            this.ClientRoleList = new List<SelectListItem>();
            //this.ClientList = new List<User>();


        }
        public List<SelectListItem> ClientRoleList { get; set; }
        //public List<User> ClientList { get; set; }
    }
}
