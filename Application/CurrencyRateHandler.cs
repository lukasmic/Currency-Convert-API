using Currency_Convert_API.Entities;
using Currency_Convert_API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return repository.GetCurrencyRates().Where(rate => rate.Currency == currency).SingleOrDefault();
        }

        public IEnumerable<string> GetCurrencyRateNames()
        {
            return repository.GetCurrencyRates().Select(r => r.Currency);
        }
    }
}
