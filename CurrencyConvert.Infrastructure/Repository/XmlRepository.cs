using CurrencyConvert.Models;
using CurrencyConvert.Tools;
using Microsoft.Extensions.Configuration;

namespace CurrencyConvert.Infrastructure.Repository
{
    public class XmlRepository : ICurrencyRatesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<CurrencyRate> _currencyRates;

        public XmlRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerable<CurrencyRate> GetCurrencyRates()
        {
            List<CurrencyRate> currencyRates = SetBaseRate();

            currencyRates.AddRange(XmlParser.GetCurrencyRates("", "CurrentRates.xml"));

            return currencyRates;
        }

        public IEnumerable<CurrencyRate> GetCurrencyRates(string date)
        {
            List<CurrencyRate> currencyRates = SetBaseRate();

            currencyRates.AddRange(XmlParser.GetCurrencyRates(
                _configuration.GetRequiredSection("HistoricalRatesFolder").Value ?? throw new ArgumentNullException("HistoricalRatesFolder was not found in appsettings.json"),
                $"currencyRates_{date}.xml"
            ));

            return currencyRates;
        }

        public IEnumerable<string> GetHistoricalDates()
        {
            var historicalRateDirectory = Path.Combine(
                Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,
                _configuration.GetRequiredSection("HistoricalRatesFolder").Value ?? throw new ArgumentNullException("HistoricalRatesFolder was not found in appsettings.json")
            );

            var files = Directory.GetFiles(historicalRateDirectory).Select(f => Path.GetFileNameWithoutExtension(f));

            return files.Select(f => f[14..]);
        }

        private static List<CurrencyRate> SetBaseRate()
        {
            return new()
            {
                new CurrencyRate() { Currency = "EUR", ToEuro = 1 }
            };
        }
    }
}
