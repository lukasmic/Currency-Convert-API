using CurrencyConvert.Models;

namespace CurrencyConvert.Application.CurrentRate
{
    public interface ICurrentRateHandler
    {
        public CurrencyRate GetCurrencyRate(string currency);

        public IEnumerable<string> GetCurrencyRateNames();
    }
}
