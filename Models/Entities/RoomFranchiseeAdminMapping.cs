using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Models.Entities
{
    public class RoomFranchiseeAdminMapping :Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int UserID { get; set; }  //Franchisee User Id
        public bool IsDelete { get; set; }
    }
}
