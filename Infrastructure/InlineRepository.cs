using Currency_Convert_API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Currency_Convert_API.Infrastructure
{
    public class InlineRepository : ICurrencyRatesRepository
    {
        private readonly List<CurrencyRate> currencyRates = new()
        {
            new CurrencyRate { Currency = "EUR", ToEuro = 1 },
            new CurrencyRate { Currency = "USD", ToEuro = 1.2066 },
            new CurrencyRate { Currency = "JPY", ToEuro = 129.98 },
            new CurrencyRate { Currency = "BGN", ToEuro = 1.9558 }
        };

        public IEnumerable<string> GetCurrencyRateNames()
        {
            return currencyRates.Select(r => r.Currency);
        }

        public CurrencyRate GetCurrencyRate(string currency)
        {
            return currencyRates.Where(rate => rate.Currency == currency).SingleOrDefault();
        }
    }
}
