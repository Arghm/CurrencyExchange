using CurrencyExchange.Contracts.Entities;
using CurrencyExchange.Contracts.Repositories.Entities;

namespace CurrencyExchange.Contracts.Repositories
{
    public interface IDbRepository
    {
        /// <summary>
        /// Returns all users.
        /// </summary>
        Task<UserEntity[]> GetAllUsers(CancellationToken cancellationToken);

        Task<UserEntity> GetUsersById(Guid userId, CancellationToken cancellationToken);

        Task<UserEntity> CreateUser(UserEntity user, CancellationToken cancellationToken);

        Task<UserEntity> UpdateUser(UserEntity user, CancellationToken cancellationToken);

        Task DeleteUser(Guid userId, CancellationToken cancellationToken);

        Task<CurrencyEntity> AddCurrency(CurrencyEntity currency, CancellationToken cancellationToken);

        Task<CurrencyEntity[]> GetAllCurrencies(CancellationToken cancellationToken);

        Task<CurrencyEntity?> GetCurrencyByCode(string code, CancellationToken cancellationToken);

        Task<UserAccountEntity[]> GetAccountsByUserId(Guid userId, CancellationToken cancellationToken);

        Task<UserAccountEntity?> GetAccountsByUserIdByCurrencyId(Guid userId, Guid currencyId, CancellationToken cancellationToken);

        Task<UserAccountEntity> CreateUserAccount(UserAccountEntity userAccountEntity, CancellationToken cancellationToken);

        Task<UserAccountEntity> ChangeUserAccountAmount(Guid userId, Guid currencyId, decimal changeAmount, CancellationToken cancellationToken);

        Task<TransactionEntity> ExchangeUserCurrency(ExchangeCurrency exchangeCurrency, CancellationToken cancellationToken);
    }
}
