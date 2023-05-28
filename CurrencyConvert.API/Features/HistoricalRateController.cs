using CurrencyConvert.Application.HistoricalRate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CurrencyConvert.Features.HistoricalRate
{
    [ApiController]
    [Route("[controller]/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class HistoricalRateController : ControllerBase
    {
        private readonly IHistoricalRateHandler handler;

        public HistoricalRateController(IHistoricalRateHandler currencyRateHandler)
        {
            handler = currencyRateHandler;
        }

        [HttpGet]
        [Route("currencies/{date}")]
        public IEnumerable<string> GetHistoricalCurrencies(string date)
        {
            var currencyRate = handler.GetCurrencyRateNames(date);

            return currencyRate;
        }

        [HttpGet("convertedCurrency/{date}/{currency}:{amount}/{targetCurrency}")]
        public ActionResult<double> GetConvertedCurrency(string currency, string targetCurrency, double amount, string date)
        {
            var rate = handler.GetCurrencyRate(currency.ToUpper(), date);
            var targetRate = handler.GetCurrencyRate(targetCurrency.ToUpper(), date);

            if (rate == null || targetRate == null)
            {
                return NotFound();
            }

            var targetAmount = amount * targetRate.ToEuro / rate.ToEuro;
            return targetAmount;
        }

        [HttpGet]
        [Route("availableDates")]
        public IEnumerable<string> GetAvailableDates()
        {
            var currencyRate = handler.GetAvailableDates();

            return currencyRate;
        }
    }
}
