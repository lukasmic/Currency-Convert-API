using System.Collections.Generic;
using Currency_Convert_API.Entities;
using Currency_Convert_API.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Currency_Convert_API.API
{
    [ApiController]
    [Route("currencyRate")]
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
            
            if (rate is null || targetRate is null)
            {
                return NotFound();
            }

            var targetAmount = amount * targetRate.ToEuro / rate.ToEuro;
            return targetAmount;
        }
    }
}
