using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Models.Entities
{
    public class RoomImage: Auditable
    {
        [Key]
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int DisplayOrder { get; set; }
        public string ImagePath { get; set; }
        public string? Description { get; set; }
        public bool IsDelete { get; set; }
        public string FileExtension { get; set; }
        public string CatalogUrl { get; set; }
        public Room RoomDetail { get; set; }
    }
}
