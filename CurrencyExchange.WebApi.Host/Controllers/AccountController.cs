using CurrencyExchange.Application.Handlers;
using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.WebApi.Host.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountHandler _accountHandler;
        public AccountController(IUserAccountHandler accountHandler)
        {
            _accountHandler = accountHandler ?? throw new ArgumentNullException(nameof(accountHandler));
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        [HttpGet]
        [Route("get-accouts-by-user-id")]
        public async Task<IActionResult> GetAllUserAccounts([FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            return Ok(await _accountHandler.GetAccountsByUserId(userId, cancellationToken));
        }

        /// <summary>
        /// Exchange currency for user.
        /// </summary>
        [HttpPost]
        [Route("change-account-sum")]
        public async Task<IActionResult> ChangeAccountAmount(ChangeAccountModel account, CancellationToken cancellationToken)
        {   
            return Ok(await _accountHandler.ChangeUserAccountAmount(account.UserId, account.CurrencyCode, account.ChangeSum, cancellationToken));
        }

        /// <summary>
        /// Exchange currency for user.
        /// </summary>
        [HttpPost]
        [Route("exchange-currency")]
        public async Task<IActionResult> ExchangeUserCurrency(ExchangeCurrencyModel exchange, CancellationToken cancellationToken)
        {
            return Ok(await _accountHandler.ExchangeUserCurrency(exchange, cancellationToken));
        }
    }
}
