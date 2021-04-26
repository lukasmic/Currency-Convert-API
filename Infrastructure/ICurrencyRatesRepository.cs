using Currency_Convert_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currency_Convert_API.Infrastructure
{
    public interface ICurrencyRatesRepository
    {
        public IEnumerable<string> GetCurrencyRateNames();
        public CurrencyRate GetCurrencyRate(string currency);
    }
}
