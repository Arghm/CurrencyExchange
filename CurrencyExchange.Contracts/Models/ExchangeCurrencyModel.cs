using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Contracts.Models
{
    public sealed class ExchangeCurrencyModel
    {
        public Guid UserId { get; set; }
        public string FromCurrencyCode { get; set; } = null!;
        public string ToCurrencyCode { get; set; } = null!;
        public decimal AmountToExchange { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ExchangeFeePercentage { get; set; } = 0.05m;
    }
}
