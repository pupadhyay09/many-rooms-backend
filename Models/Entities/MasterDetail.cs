using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class MasterDetail : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string? OtherName { get; set; }
        public string? Description { get; set; }
        public int? ParentID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public List<RoomEvent> RoomEventList { get; set; }
    }
}
