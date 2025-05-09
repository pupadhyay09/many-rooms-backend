using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Models.Entities
{
    public class UserBankDetail :Auditable
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        public string? BankName { get; set; }
        public int? AccountNumber { get; set; }
        public string? SortCode { get; set; }
        public bool IsDelete { get; set; }
    }
}
