using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Contracts.Repositories.Entities
{
    [Table("transaction", Schema = "public")]
    public class TransactionEntity
    {
        [Key]
        [Column("transaction_id"), Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; }

        [Column("from_account_id"), Required]
        public Guid FromAccountId { get; set; }
        public UserAccountEntity FromAccount { get; set; } = null!;

        [Column("exchange_amount"), Required]
        public decimal ExchangeAmount { get; set; }

        [Column("to_account_id"), Required]
        public Guid ToAccountId { get; set; }
        public UserAccountEntity ToAccount { get; set; } = null!;

        [Column("exchange_rate"), Required]
        public decimal ExchangeRate { get; set; }

        [Column("exchange_fee"), Required]
        public decimal ExchangeFee { get; set; }

        [Column("careated_on"), Required]
        public DateTime CreatedOn { get; set; }
    }
}
