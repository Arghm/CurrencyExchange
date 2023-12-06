using CurrencyExchange.Application.Handlers;
using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Models;
using CurrencyExchange.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CurrencyExchange.WebApi.Host.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserHandler _userHandler;

        public UserController(IUserHandler userHandler)
        {
            _userHandler = userHandler ?? throw new ArgumentNullException(nameof(userHandler));
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel user, CancellationToken cancellationToken)
        {
            var userModel = await _userHandler.CreateUser(user, cancellationToken);
            return Ok(userModel);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userHandler.GetAllUsers(cancellationToken);
            return Ok(users);
        }

        /// <summary>
        /// Get user by user Id.
        /// </summary>
        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<IActionResult> GetUserByUserId([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            var userModel = await _userHandler.GetUserByUserId(userId, cancellationToken);
            return Ok(userModel);
        }

        /// <summary>
        /// Update user data.
        /// </summary>
        [HttpPut]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel user, CancellationToken cancellationToken)
        {
            await _userHandler.UpdateUser(user, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Update user data.
        /// </summary>
        [HttpDelete]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            await _userHandler.DeleteUser(userId, cancellationToken);
            return NoContent();
        }
    }
}
