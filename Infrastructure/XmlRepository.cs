using Currency_Convert_API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Currency_Convert_API.Infrastructure
{
    public class XmlRepository : ICurrencyRatesRepository
    {
        public IEnumerable<CurrencyRate> ReadFromFile()
        {
            List<CurrencyRate> currencyRates = new();
            currencyRates.Add(new CurrencyRate() { Currency = "EUR", ToEuro = 1 });
            XmlReader reader = XmlReader.Create("C:\\eurofxref-daily.xml");
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
            return currencyRates;
        }

        public CurrencyRate GetCurrencyRate(string currency)
        {
            return ReadFromFile().Where(rate => rate.Currency == currency).SingleOrDefault();
        }

        public IEnumerable<string> GetCurrencyRateNames()
        {
            return ReadFromFile().Select(r => r.Currency);
        }
    }
}
