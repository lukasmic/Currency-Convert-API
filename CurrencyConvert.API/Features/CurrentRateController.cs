using CurrencyConvert.Application.CurrentRate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CurrencyConvert.Features.CurrentRate
{
    [ApiController]
    [Route("[controller]/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class CurrentRateController : ControllerBase
    {
        private readonly ICurrentRateHandler handler;

        public CurrentRateController(ICurrentRateHandler currencyRateHandler)
        {
            handler = currencyRateHandler;
        }

        [HttpGet]
        [Route("currencies")]
        public IEnumerable<string> GetCurrencies()
        {
            var currencyRate = handler.GetCurrencyRateNames();

            return currencyRate;
        }

        [HttpGet("convertedCurrency/{currency}:{amount}/{targetCurrency}")]
        public ActionResult<double> GetConvertedCurrency(string currency, string targetCurrency, double amount)
        {
            var rate = handler.GetCurrencyRate(currency.ToUpper());
            var targetRate = handler.GetCurrencyRate(targetCurrency.ToUpper());

            if (rate == null || targetRate == null)
            {
                return NotFound();
            }

            var targetAmount = amount * targetRate.ToEuro / rate.ToEuro;
            return targetAmount;
        }
    }
}
