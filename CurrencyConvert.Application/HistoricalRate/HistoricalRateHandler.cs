using CurrencyConvert.Infrastructure.Repository;
using CurrencyConvert.Models;
using Microsoft.Extensions.Configuration;

namespace CurrencyConvert.Application.HistoricalRate
{
    public class HistoricalRateHandler : IHistoricalRateHandler
    {
        private readonly ICurrencyRatesRepository _repository;
        private readonly IConfiguration _configuration;

        public HistoricalRateHandler(ICurrencyRatesRepository repository, IConfiguration configuration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public CurrencyRate GetCurrencyRate(string currency, string date)
        {
            return _repository.GetCurrencyRates(date).SingleOrDefault(rate => rate.Currency == currency);
        }

        public IEnumerable<string> GetCurrencyRateNames(string date)
        {
            return _repository.GetCurrencyRates(date).Select(r => r.Currency);
        }

        public IEnumerable<string> GetAvailableDates()
        {
            return _repository.GetHistoricalDates();
        }
    }
}
