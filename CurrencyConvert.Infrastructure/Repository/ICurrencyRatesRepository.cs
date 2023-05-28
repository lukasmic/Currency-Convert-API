using CurrencyConvert.Models;

namespace CurrencyConvert.Infrastructure.Repository
{
    public interface ICurrencyRatesRepository
    {
        public IEnumerable<CurrencyRate> GetCurrencyRates();

        public IEnumerable<CurrencyRate> GetCurrencyRates(string date);

        public IEnumerable<string> GetHistoricalDates();
    }
}
