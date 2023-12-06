using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyExchange.Contracts.Repositories.Entities
{
    [Table("user_account", Schema = "public")]
    public class UserAccountEntity
    {
        [Key]
        [Column("account_id"), Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountId { get; set; }

        [Column("user_id"), Required]
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        [Column("currency_id"), Required]
        public Guid CurrencyId { get; set; }
        public CurrencyEntity Currency { get; set; } = null!;

        [Column("total_sum"), Required]
        public decimal TotalSum { get; set; }
    }
}
