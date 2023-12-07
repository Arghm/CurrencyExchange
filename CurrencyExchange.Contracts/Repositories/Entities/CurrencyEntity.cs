using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyExchange.Contracts.Repositories.Entities
{
    [Table("currency", Schema = "public")]
    public class CurrencyEntity
    {
        [Key]
        [Column("currency_id"), Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CurrencyId { get; set; }

        [Column("code", TypeName = "text"), Required]
        public string Code { get; set; } = null!;

        [Column("name", TypeName = "text"), Required]
        public string Name { get; set; } = null!;

        public List<UserAccountEntity> Account { get; set; } = new();
    }
}
