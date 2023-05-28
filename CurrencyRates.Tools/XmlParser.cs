using CurrencyConvert.Models;
using System.Xml;

namespace CurrencyConvert.Tools
{
    public class XmlParser
    {
        public static IEnumerable<CurrencyRate> ParseCurrencyRates(string folder, string xml)
        {
            var currencyRates = new List<CurrencyRate>();

            var solutionFolder = Directory.GetParent(Directory.GetCurrentDirectory());

            var fileDirectory = folder == "" ?
                Path.Combine(solutionFolder!.FullName, $"{xml}") :
                Path.Combine(solutionFolder!.FullName, $"{folder}", $"{xml}");

            if (!File.Exists(fileDirectory))
            {
                Console.WriteLine($"File {fileDirectory} does not exist");
                return currencyRates;
            }

            using (StreamReader sr = new StreamReader(fileDirectory, new FileStreamOptions { Access = FileAccess.Read }))
            {
                var reader = XmlReader.Create(sr);
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
            }

            return currencyRates;
        }
    }
}
