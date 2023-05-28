using CurrencyConvert.Models;

namespace CurrencyConvert.Infrastructure.Repository
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

        public IEnumerable<CurrencyRate> GetCurrencyRates(string date)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetHistoricalDates()
        {
            throw new NotImplementedException();
        }
    }
}
