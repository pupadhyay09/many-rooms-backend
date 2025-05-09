using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    
    public class Room : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? RoomName { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyPrice { get; set; }

        public int DiscountPercentage { get; set; }
        public int VATPercentage { get; set; }
        public int CommissionPercentage { get; set; }
        public bool IsVATEnabled { get; set; }
        public int Capacity { get; set; }
        public int TotalBeds { get; set; }
        public int TotalSofas { get; set; }
        public bool IsDelete { get; set; }

        [ForeignKey("MasterDetail")]
        public int OwnershipTypeID { get; set; }
        public ICollection<RoomImage> RoomImages { get; set; }
        public ICollection<RoomEvent> RoomEvents { get; set; }
        public MasterDetail OwnershipType { get; set; }
    }
}
