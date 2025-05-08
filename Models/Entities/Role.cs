using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManyRoomStudio.Models.Entities
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? RoleName { get; set; }
        public bool IsDelete { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
