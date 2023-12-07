using CurrencyExchange.Contracts.Models;

namespace CurrencyExchange.Contracts.Handlers
{
    /// <summary>
    /// User operations handler.
    /// </summary>
    public interface IUserHandler
    {
        /// <summary>
        /// Get all users data.
        /// </summary>
        Task<UserModel[]> GetAllUsers(CancellationToken cancellationToken);

        /// <summary>
        /// Get user data by user Id.
        /// </summary>
        Task<UserModel> GetUserByUserId(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <returns>user Id</returns>
        Task<UserModel> CreateUser(UserModel user, CancellationToken cancellationToken);

        /// <summary>
        /// Update user data.
        /// </summary>
        Task UpdateUser(UserModel user, CancellationToken cancellationToken);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="userId"></param>
        Task DeleteUser(Guid userId, CancellationToken cancellationToken);
    }
}
