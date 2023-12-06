using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Contracts.Models
{
    public sealed class ExchangeTransactionModel
    {
        public Guid TransactionId { get; set; }
        public Guid ToAccountId { get; set; }
        public Guid FromAccountId { get; set; }
        public decimal ExchangeAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ExchangeFee { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
