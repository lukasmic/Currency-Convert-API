using Currency_Convert_API.Models;
using CurrencyConvert.Infrastructure.Repository;

namespace Currency_Convert_API.Application
{
    public class CurrencyRateHandler : ICurrencyRateHandler
    {
        private readonly ICurrencyRatesRepository repository;

        public CurrencyRateHandler(ICurrencyRatesRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public CurrencyRate GetCurrencyRate(string currency)
        {
            return repository.GetCurrencyRates().SingleOrDefault(rate => rate.Currency == currency);
        }

        public IEnumerable<string> GetCurrencyRateNames()
        {
            return repository.GetCurrencyRates().Select(r => r.Currency);
        }
    }
}
