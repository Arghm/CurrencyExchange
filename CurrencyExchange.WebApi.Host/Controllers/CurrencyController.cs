using CurrencyExchange.Contracts.Handlers;
using CurrencyExchange.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace CurrencyExchange.WebApi.Host.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyHandler _currencyHandler;
        public CurrencyController(ICurrencyHandler currencyHandler)
        {
            _currencyHandler = currencyHandler ?? throw new ArgumentNullException(nameof(currencyHandler));
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        [HttpGet]
        [Route("get-all-currencies")]
        public async Task<IActionResult> GetAllCurrencies(CancellationToken cancellationToken)
        {
            return Ok(await _currencyHandler.GetAllCurrencies(cancellationToken));
        }

        /// <summary>
        /// Add new currency.
        /// </summary>
        [HttpPost]
        [Route("add-currency")]
        public async Task<IActionResult> AddCurrency([FromBody] CurrencyModel currency, CancellationToken cancellationToken)
        {
            await _currencyHandler.AddCurrency(currency, cancellationToken);
            return NoContent();
        }
    }
}
