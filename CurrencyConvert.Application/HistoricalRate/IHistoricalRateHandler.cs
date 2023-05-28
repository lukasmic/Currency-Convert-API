using CurrencyConvert.Models;

namespace CurrencyConvert.Application.HistoricalRate
{
    public interface IHistoricalRateHandler
    {
        public CurrencyRate GetCurrencyRate(string currency, string date);

        public IEnumerable<string> GetCurrencyRateNames(string date);

        public IEnumerable<string> GetAvailableDates();
    }
}
