using CurrencyExchange.Contracts.Entities;
using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Models;
using CurrencyExchange.Contracts.Repositories;
using CurrencyExchange.Contracts.Repositories.Entities;
using System.Data;

namespace CurrencyExchange.Application.Handlers
{
    public sealed class UserAccountHandler : IUserAccountHandler
    {
        private readonly ICurrencyHandler _currencyHandler;
        private readonly IUserHandler _userHandler;
        private readonly IDbRepository _dbRepository;
        public UserAccountHandler(IDbRepository dbRepository,
            IUserHandler userHandler,
            ICurrencyHandler currencyHandler)
        {
            _dbRepository = dbRepository ?? throw new ArgumentNullException(nameof(dbRepository));
            _currencyHandler = currencyHandler ?? throw new ArgumentNullException(nameof(currencyHandler));
            _userHandler = userHandler ?? throw new ArgumentNullException(nameof(_userHandler));
        }

        public async Task<UserAccountModel> GetAccountsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var accounts = await _dbRepository.GetAccountsByUserId(userId, cancellationToken);
            return Map(accounts);
        }

        public async Task<AccountModel> ChangeUserAccountAmount(Guid userId, string currencyCode, decimal changeAmount, CancellationToken cancellationToken)
        {
            var currency = await _currencyHandler.GetCurrencyByCode(currencyCode, cancellationToken);
            var user = await _userHandler.GetUserByUserId(userId, cancellationToken);
            var account = await _dbRepository.ChangeUserAccountAmount(user.UserId, currency.CurrencyId, changeAmount, cancellationToken);
            return MapAccountEntityToModel(account);
        }

        public async Task<ExchangeTransactionModel> ExchangeUserCurrency(ExchangeCurrencyModel exchange, CancellationToken cancellationToken)
        {
            Validate(exchange);
            var toCurrency = await _currencyHandler.GetCurrencyByCode(exchange.ToCurrencyCode, cancellationToken);
            var fromCurrency = await _currencyHandler.GetCurrencyByCode(exchange.FromCurrencyCode, cancellationToken);

            // сумма в новой валюте
            var exchangeToAmount = exchange.AmountToExchange / exchange.ExchangeRate;
            // комиссия = сумма в новой валюте * процет комиссии
            var exchangeFee = exchangeToAmount * (exchange.ExchangeFeePercentage / 100);
            // сумма в новой валюте за вычетом комиссии
            exchangeToAmount = exchangeToAmount - exchangeFee;

            var exchangeCurrency = MapToExchangeCurrency(exchange,
                fromCurrency.CurrencyId,
                toCurrency.CurrencyId,
                exchangeToAmount);
            var transaction = await _dbRepository.ExchangeUserCurrency(exchangeCurrency, cancellationToken);
            return MapTransactionToModel(transaction);
        }

        private void Validate(ExchangeCurrencyModel exchange)
        {
            if (exchange == null)
            {
                throw new ArgumentNullException(nameof(exchange));
            }

            if (string.IsNullOrWhiteSpace(exchange.FromCurrencyCode))
            {
                throw new ArgumentNullException(nameof(exchange.FromCurrencyCode));
            }

            if (string.IsNullOrWhiteSpace(exchange.ToCurrencyCode))
            {
                throw new ArgumentNullException(nameof(exchange.ToCurrencyCode));
            }

            if (exchange.AmountToExchange <= 0)
            {
                throw new ArgumentException(nameof(exchange.AmountToExchange));
            }

            if (exchange.ExchangeRate <= 0)
            {
                throw new ArgumentException(nameof(exchange.ExchangeRate));
            }
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="fromCurrencyId">id валюты которую обменивают</param>
        /// <param name="toCurrencyId">id валюты на которую обменивают</param>
        /// <param name="ExchangeToAmount">Сумма зачисления на счет на который переводятся деньги.</param>
        private ExchangeCurrency MapToExchangeCurrency(ExchangeCurrencyModel source,
            Guid fromCurrencyId,
            Guid toCurrencyId,
            decimal ExchangeToAmount)
        {
            return new ExchangeCurrency
            {
                ExchangeFeePercentage = source.ExchangeFeePercentage,
                FromCurrencyId = fromCurrencyId,
                ToCurrencyId = toCurrencyId,
                ExchangeToAmount = ExchangeToAmount,
                ExchangeFromAmount = source.AmountToExchange,
                UserId = source.UserId,
                ExcangeRate = source.ExchangeRate,
            };
        }

        private TransactionEntity MapToTransactionEntity(Guid toAccountId, Guid fromAccountId, ExchangeCurrencyModel exchange)
        {
            return new TransactionEntity
            {
                ToAccountId = toAccountId,
                FromAccountId = fromAccountId,
                ExchangeAmount = exchange.AmountToExchange,
                ExchangeRate = exchange.ExchangeRate,
                ExchangeFee = exchange.ExchangeFeePercentage,
                CreatedOn = DateTime.UtcNow,
            };
        }

        private ExchangeTransactionModel MapTransactionToModel(TransactionEntity source)
        {
            return new ExchangeTransactionModel
            {
                TransactionId = source.TransactionId,
                FromAccountId = source.FromAccountId,
                ToAccountId = source.ToAccountId,
                ExchangeAmount = source.ExchangeAmount,
                ExchangeRate = source.ExchangeRate,
                ExchangeFee = source.ExchangeFee,
                CreatedOn = source.CreatedOn,
            };
        }

        private UserAccountModel Map(UserAccountEntity[] source)
        {
            UserEntity user = source.FirstOrDefault().User;
            var result = new UserAccountModel
            {
                UserId = user.UserId,
                UserLogin = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            result.Accounts = source.Select(MapAccountEntityToModel).ToArray();
            return result;
        }

        private AccountModel MapAccountEntityToModel(UserAccountEntity source)
        {
            return new AccountModel
            {
                AccountId = source.AccountId,
                CurrencyCode = source.Currency.Code,
                CurrencyName = source.Currency.Name,
                TotalSum = source.TotalSum,
            };
        }

        private UserAccountEntity MapToEntity(Guid userId, Guid currencyId)
        {
            return new UserAccountEntity
            {
                UserId = userId,
                CurrencyId = currencyId,
                TotalSum = 0,
            };
        }
    }
}
