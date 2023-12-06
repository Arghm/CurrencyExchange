using CurrencyExchange.Contracts.Entities;
using CurrencyExchange.Contracts.Repositories;
using CurrencyExchange.Contracts.Repositories.Entities;
using CurrencyExchange.Migrations;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Application.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly CurrencyExchangeContext _dbContext;
        public DbRepository(CurrencyExchangeContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<UserEntity> CreateUser(UserEntity user, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                var existedUser = await GetUsersById(user.UserId, cancellationToken);
                if (existedUser == null)
                {
                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    dbContextTransaction.Commit();
                    return user;
                }
                return existedUser;
            }
        }

        public async Task DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AsNoTracking().AnyAsync(u => u.UserId == userId, cancellationToken))
            {
                _dbContext.Users.Remove(new UserEntity { UserId = userId });
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<UserEntity[]> GetAllUsers(CancellationToken cancellationToken)
        {
            return _dbContext.Users.AsNoTracking().ToArrayAsync(cancellationToken);
        }

        public Task<UserEntity> GetUsersById(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.Users.FirstOrDefaultAsync(f => f.UserId == userId, cancellationToken);
        }

        public async Task<UserEntity> UpdateUser(UserEntity user, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                var existedUser = await _dbContext.Users.FirstOrDefaultAsync(f => f.UserId == user.UserId, cancellationToken);
                if (existedUser != null)
                {
                    existedUser.Login = user.Login;
                    existedUser.FirstName = user.FirstName;
                    existedUser.LastName = user.LastName;
                    existedUser.UpdatedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                return user;
            }
        }

        public Task<CurrencyEntity[]> GetAllCurrencies(CancellationToken cancellationToken)
        {
            return _dbContext.Currencies.ToArrayAsync(cancellationToken);
        }

        public async Task<CurrencyEntity> AddCurrency(CurrencyEntity currency, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                var currencyEntity = await _dbContext.Currencies.AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Code == currency.Code);
                if (currencyEntity != null)
                {
                    return currencyEntity;
                }

                _dbContext.Currencies.Add(currency);
                await _dbContext.SaveChangesAsync(cancellationToken);
                dbContextTransaction.Commit();
                return currency;
            }
        }

        public Task<CurrencyEntity?> GetCurrencyByCode(string code, CancellationToken cancellationToken)
        {
            return _dbContext.Currencies.FirstOrDefaultAsync(f => f.Code == code);
        }

        public async Task<UserAccountEntity[]> GetAccountsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.UserAccounts.AsNoTracking()
                .Where(w => w.UserId == userId)
                .Include(u => u.User)
                .Include(c => c.Currency)
                .ToArrayAsync(cancellationToken);
            return accounts;
        }

        public Task<UserAccountEntity?> GetAccountsByUserIdByCurrencyId(Guid userId, Guid currencyId, CancellationToken cancellationToken)
        {
            return _dbContext.UserAccounts
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CurrencyId == currencyId, cancellationToken);
        }

        public async Task<UserAccountEntity> CreateUserAccount(UserAccountEntity userAccountEntity, CancellationToken cancellationToken)
        {
            _dbContext.UserAccounts.Add(userAccountEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return userAccountEntity;
        }

        public async Task<UserAccountEntity> ChangeUserAccountAmount(Guid userId, Guid currencyId, decimal changeAmount, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var account = await _dbContext.UserAccounts
                        .FirstOrDefaultAsync(f => f.UserId == userId && f.CurrencyId == currencyId, cancellationToken);
                    if (account == null) 
                    {
                        account = MapAccountEntity(userId, currencyId);
                        _dbContext.UserAccounts.Add(account);
                    }
                    if (account.TotalSum + changeAmount < 0)
                    {
                        throw new ArgumentException(nameof(changeAmount));
                    }
                    account.TotalSum += changeAmount;
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    dbContextTransaction.Commit();
                    return account;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw new Exception("Ошибка изменения счета", ex);
                }
            }
        }

        public async Task<TransactionEntity> ExchangeUserCurrency(ExchangeCurrency exchange, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var fromAccount = await _dbContext.UserAccounts
                        .FirstOrDefaultAsync(f => f.UserId == exchange.UserId && f.CurrencyId == exchange.FromCurrencyId, cancellationToken);
                    if (fromAccount == null
                        || exchange.ExchangeFromAmount > fromAccount.TotalSum)
                    {
                        throw new ArgumentException("Недостаточная сумма на счете");
                    }

                    var toAccount = await _dbContext.UserAccounts
                        .FirstOrDefaultAsync(f => f.UserId == exchange.UserId && f.CurrencyId == exchange.ToCurrencyId, cancellationToken);
                    if (toAccount == null)
                    {
                        toAccount = MapAccountEntity(exchange.UserId, exchange.ToCurrencyId);
                        _dbContext.UserAccounts.Add(toAccount);
                    }

                    toAccount.TotalSum += exchange.ExchangeToAmount;
                    fromAccount.TotalSum -= exchange.ExchangeFromAmount;

                    var transactionEntity = MapToTransactionEntity(toAccount.AccountId, fromAccount.AccountId, exchange);
                    _dbContext.Transactions.Add(transactionEntity);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    dbContextTransaction.Commit();
                    return transactionEntity;
                }
                catch (Exception ex) 
                {
                    dbContextTransaction.Rollback();
                    throw new Exception("Ошибка обмена валют.", ex);
                }
            }
        }

        private TransactionEntity MapToTransactionEntity(Guid toAccountId, Guid fromAccountId, ExchangeCurrency exchange)
        {
            return new TransactionEntity
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                ExchangeAmount = exchange.ExchangeFromAmount,
                ExchangeRate = exchange.ExcangeRate,
                ExchangeFee = exchange.ExchangeFeePercentage,
                CreatedOn = DateTime.UtcNow,
            };
        }

        private UserAccountEntity MapAccountEntity(Guid userId, Guid currencyId)
        {
            return new UserAccountEntity
            {
                CurrencyId = currencyId,
                UserId = userId,
                TotalSum = 0,
            };
        }
    }
}
