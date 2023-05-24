using Currency_Convert_API.Entities;
using System.Collections.Generic;

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

        public IEnumerable<CurrencyRate> GetCurrencyRates()
        {
            return currencyRates;
        }
    }
}
