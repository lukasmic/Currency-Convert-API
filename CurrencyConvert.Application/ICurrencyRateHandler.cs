using Currency_Convert_API.Models;

namespace Currency_Convert_API.Application
{
    public interface ICurrencyRateHandler
    {
        public CurrencyRate GetCurrencyRate(string currency);

        public IEnumerable<string> GetCurrencyRateNames();
    }
}
