using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Models.Entities
{
    public class Franchiseekey :Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string? Publishablekey { get; set; }
        public string? Secretkey { get; set; }
        public string? Paymnetkeytype { get; set; }
        public bool IsDelete { get; set; }
    }
}
