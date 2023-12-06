using CurrencyExchange.Contracts.Models;
using CurrencyExchange.Contracts.Repositories.Entities;

namespace CurrencyExchange.Contracts.Handlers
{
    public interface ICurrencyHandler
    {
        Task<CurrencyModel[]> GetAllCurrencies(CancellationToken cancellationToken);
        Task AddCurrency(CurrencyModel currency, CancellationToken cancellationToken);
        Task<CurrencyEntity> GetCurrencyByCode(string code, CancellationToken cancellationToken);
    }
}
