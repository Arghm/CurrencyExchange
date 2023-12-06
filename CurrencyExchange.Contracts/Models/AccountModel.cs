using CurrencyExchange.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.Contracts.Models
{
    public sealed class AccountModel
    {
        public Guid AccountId { get; set; }

        public string CurrencyCode { get; set; } = null!;

        public string CurrencyName { get; set; } = null!;

        public decimal TotalSum { get; set; }
    }
}
