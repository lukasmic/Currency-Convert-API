using Currency_Convert_API.Entities;

namespace CurrencyConvert.Infrastructure.Repository
{
    public interface ICurrencyRatesRepository
    {
        public IEnumerable<CurrencyRate> GetCurrencyRates();
    }
}
