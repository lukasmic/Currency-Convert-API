using Currency_Convert_API.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Currency_Convert_API.Features
{
    [ApiController]
    [Route("currencyRate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class CurrencyRateController : ControllerBase
    {
        private readonly ICurrencyRateHandler handler;

        public CurrencyRateController(ICurrencyRateHandler currencyRateHandler)
        {
            handler = currencyRateHandler;
        }

        [HttpGet]
        [Route("/currencies")]
        public IEnumerable<string> GetCurrencies()
        {
            var currencyRate = handler.GetCurrencyRateNames();

            return currencyRate;
        }

        [HttpGet("/convertedCurrency/{currency}:{amount}/{targetCurrency}")]
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
