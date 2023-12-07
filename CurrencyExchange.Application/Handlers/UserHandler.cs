using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Models;
using CurrencyExchange.Contracts.Repositories;
using CurrencyExchange.Contracts.Repositories.Entities;
using Microsoft.Extensions.Logging;
using System.Data;

namespace CurrencyExchange.Application.Handlers
{
    public sealed class UserHandler : IUserHandler
    {
        private readonly IDbRepository _dbRepository;
        private readonly ILogger<UserHandler> _logger;

        public UserHandler(IDbRepository dbRepository,
            ILogger<UserHandler> logger)
        {
            _dbRepository = dbRepository ?? throw new ArgumentNullException(nameof(dbRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<UserModel> CreateUser(UserModel user, CancellationToken cancellationToken)
        {
            var result = await _dbRepository.CreateUser(MapModelToEntity(user), cancellationToken);
            return MapUserEntityToModel(result);
        }

        public Task DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            return _dbRepository.DeleteUser(userId, cancellationToken);
        }

        public async Task<UserModel[]> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _dbRepository.GetAllUsers(cancellationToken);
                return Map(users);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "GetAllUsers error");
                throw;
            }
        }

        public async Task<UserModel> GetUserByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _dbRepository.GetUsersById(userId, cancellationToken);
            if (user == null) 
            {
                throw new ArgumentException($"User не найден: {userId}");
            }
            return MapUserEntityToModel(user);
        }

        public async Task UpdateUser(UserModel user, CancellationToken cancellationToken)
        {
            if (user.UserId == Guid.Empty)
            {
                throw new ArgumentException(nameof(user.UserId));
            }
            await _dbRepository.UpdateUser(MapModelToEntity(user), cancellationToken);
        }


        private UserModel[] Map(UserEntity[] source)
        {
            return source.Select(MapUserEntityToModel).ToArray();
        }

        private UserModel MapUserEntityToModel(UserEntity source)
        {
            return new UserModel
            {
                UserId = source.UserId,
                UserLogin = source.Login,
                LastName = source.LastName,
                FirstName = source.FirstName,
            };
        }

        private UserEntity MapModelToEntity(UserModel source)
        {
            return new UserEntity
            {
                UserId = source.UserId,
                Login = source.UserLogin,
                LastName = source.LastName,
                FirstName = source.FirstName,
            };
        }
    }
}
