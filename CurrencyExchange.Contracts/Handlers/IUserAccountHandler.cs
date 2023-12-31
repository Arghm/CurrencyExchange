﻿using CurrencyExchange.Contracts.Models;

namespace CurrencyExchange.Contracts.Handlers
{
    public interface IUserAccountHandler
    {
        Task<UserAccountModel> GetAccountsByUserId(Guid userId, CancellationToken cancellationToken);

        Task<AccountModel> ChangeUserAccountAmount(Guid userId, string currencyCode, decimal changeAmount, CancellationToken cancellationToken);

        Task<ExchangeTransactionModel> ExchangeUserCurrency(ExchangeCurrencyModel exchange, CancellationToken cancellationToken);
    }
}
