using CurrencyConvert.Infrastructure.Repository;
using CurrencyConvert.Models;

namespace CurrencyConvert.Application.CurrentRate
{
    public class CurrentRateHandler : ICurrentRateHandler
    {
        private readonly ICurrencyRatesRepository repository;

        public CurrentRateHandler(ICurrencyRatesRepository repository)
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
