using Currency_Convert_API.Models;

namespace CurrencyConvert.Infrastructure.Repository
{
    public interface ICurrencyRatesRepository
    {
        public IEnumerable<CurrencyRate> GetCurrencyRates();
    }
}
