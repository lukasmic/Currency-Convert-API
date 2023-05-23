using Currency_Convert_API.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Currency_Convert_API.API
{
    [ApiController]
    [Route("currencyRate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class CurrencyRateController : ControllerBase
    {
        private readonly ICurrencyRatesRepository repository;

        public CurrencyRateController(ICurrencyRatesRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("/currencies")]
        public IEnumerable<string> GetCurrencies()
        {
            var currencyRate = repository.GetCurrencyRateNames();

            return currencyRate;
        }

        [HttpGet("/convertedCurrency/{currency}:{amount}/{targetCurrency}")]
        public ActionResult<double> GetConvertedCurrency(string currency, string targetCurrency, double amount)
        {
            var rate = repository.GetCurrencyRate(currency.ToUpper());
            var targetRate = repository.GetCurrencyRate(targetCurrency.ToUpper());

            if (rate == null || targetRate == null)
            {
                return NotFound();
            }

            var targetAmount = amount * targetRate.ToEuro / rate.ToEuro;
            return targetAmount;
        }
    }
}
