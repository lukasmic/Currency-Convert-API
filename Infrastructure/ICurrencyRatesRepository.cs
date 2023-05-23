using Currency_Convert_API.Entities;
using System.Collections.Generic;

namespace Currency_Convert_API.Infrastructure
{
    public interface ICurrencyRatesRepository
    {
        public IEnumerable<string> GetCurrencyRateNames();

        public CurrencyRate GetCurrencyRate(string currency);
    }
}
