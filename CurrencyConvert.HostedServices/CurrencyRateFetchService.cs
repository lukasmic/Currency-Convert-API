using CurrencyConvert.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyConvert.HostedServices
{
    public class CurrencyRateFetchService : IHostedService
    {
        private readonly ILogger<CurrencyRateFetchService> _logger;
        private int _downloadFrequency;
        private string _historicalRatesFileLocation;
        private string _currentRatesFileLocation;
        private IConfiguration _configuration;

        public CurrencyRateFetchService(IConfiguration configuration, ILogger<CurrencyRateFetchService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _downloadFrequency = int.Parse(configuration.GetSection("RatesDownloadFrequencyInHours").Value ?? throw new ArgumentNullException("RatesDownloadFrequencyInHours was not found in appsettings.json"));
            _historicalRatesFileLocation = configuration.GetSection("HistoricalRatesFolder").Value ?? throw new ArgumentNullException("HistoricalRatesFolder was not found in appsettings.json");
            _currentRatesFileLocation = configuration.GetSection("LatestRatesFileLocation").Value ?? throw new ArgumentNullException("LatestRatesFileLocation was not found in appsettings.json");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Timer((e) => GetRatesFromHttp(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_downloadFrequency));

            return Task.CompletedTask;
        }

        private async void GetRatesFromHttp()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var date = XmlParser.ParseDate(responseString);
                SaveFileAsCurrentRates(responseString);
                SaveFileAsHistoricalRates(responseString, date);

                _logger.LogInformation("Currency rates downloaded and saved.");
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Error downloading currency rates. Trying again. {e}");

                Task.Delay(TimeSpan.FromMinutes(5)).Wait();
                GetRatesFromHttp();
            }
            catch (IOException e)
            {
                _logger.LogError($"Error saving currency rates to file. Trying again. {e}");

                Task.Delay(TimeSpan.FromMinutes(5)).Wait();
                GetRatesFromHttp();
            }
        }

        private void SaveFileAsCurrentRates(string downloadedRates)
        {
            var path = "../" + _currentRatesFileLocation;

            using StreamWriter outputFile = File.CreateText(Path.Combine(path, "CurrentRates.xml"));
            outputFile.WriteLine(downloadedRates);
        }

        private void SaveFileAsHistoricalRates(string downloadedRates, string date)
        {
            if (IsAlreadySavedAsHistorical(date))
                return;

            var path = "../" + _historicalRatesFileLocation;

            using StreamWriter outputFile = File.CreateText(Path.Combine(path, $"currencyRates_{date}.xml"));
            outputFile.WriteLine(downloadedRates);
        }

        private bool IsAlreadySavedAsHistorical(string date)
        {
            var historicalRateDirectory = Path.Combine(
                               Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,
                                              _configuration.GetRequiredSection("HistoricalRatesFolder").Value ?? throw new ArgumentNullException("HistoricalRatesFolder was not found in appsettings.json")
                                                         );

            return Directory.GetFiles(historicalRateDirectory).Any(f => Path.GetFileNameWithoutExtension(f).EndsWith(date));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}