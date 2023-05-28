using Currency_Convert_API.Entities;
using System.Xml;

namespace CurrencyConvert.Infrastructure.Repository
{
    public class XmlRepository : ICurrencyRatesRepository
    {
        private readonly IEnumerable<CurrencyRate> _currencyRates;

        public XmlRepository()
        {
            List<CurrencyRate> currencyRates = SetBaseRate();

            var reader = XmlReader.Create("eurofxref-daily.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Cube" && reader.GetAttribute("currency") != null)
                {
                    var currencyRate = new CurrencyRate()
                    {
                        Currency = reader.GetAttribute("currency"),
                        ToEuro = double.Parse(reader.GetAttribute("rate"))
                    };

                    currencyRates.Add(currencyRate);
                }
            }
            _currencyRates = currencyRates;
        }

        public IEnumerable<CurrencyRate> GetCurrencyRates()
        {
            return _currencyRates;
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
