using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Contracts.Models
{
    public sealed class CurrencyModel
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
