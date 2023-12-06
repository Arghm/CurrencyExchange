using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Models;
using CurrencyExchange.Contracts.Repositories;
using CurrencyExchange.Contracts.Repositories.Entities;

namespace CurrencyExchange.Application.Handlers
{
    public sealed class CurrencyHandler : ICurrencyHandler
    {
        private readonly IDbRepository _dbRepository;
        public CurrencyHandler(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository ?? throw new ArgumentNullException(nameof(dbRepository));
        }

        public Task AddCurrency(CurrencyModel currency, CancellationToken cancellationToken)
        {
            ValidateCurrency(currency);
            return _dbRepository.AddCurrency(MapModelToEntity(currency), cancellationToken);
        }

        public async Task<CurrencyModel[]> GetAllCurrencies(CancellationToken cancellationToken)
        {
            var currencies = await _dbRepository.GetAllCurrencies(cancellationToken);
            return Map(currencies);
        }

        public async Task<CurrencyEntity> GetCurrencyByCode(string code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            // TODO: add cache storage for currencies
            var currency = await _dbRepository.GetCurrencyByCode(code, cancellationToken);
            if (currency == null)
            {
                throw new ArgumentException(nameof(code));
            }
            return currency;
        }

        private void ValidateCurrency(CurrencyModel currency)
        {
            if (currency == null) 
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (string.IsNullOrWhiteSpace(currency.Code))
            {
                throw new ArgumentNullException(nameof(currency.Code));
            }

            if (string.IsNullOrWhiteSpace(currency.Name))
            {
                throw new ArgumentNullException(nameof(currency.Name));
            }
        }

        private CurrencyEntity MapModelToEntity(CurrencyModel source)
        {
            return new CurrencyEntity
            {
                Name = source.Name,
                Code = source.Code,
            };
        }

        private CurrencyModel[] Map(CurrencyEntity[] currencies)
        {
            return currencies.Select(MapEntityToModel).ToArray();
        }

        private CurrencyModel MapEntityToModel(CurrencyEntity source)
        {
            return new CurrencyModel 
            { 
                Name = source.Name, 
                Code = source.Code, 
            };
        }
    }
}
