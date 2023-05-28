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

        public CurrencyRateFetchService(IConfiguration configuration, ILogger<CurrencyRateFetchService> logger)
        {
            _logger = logger;
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
                SaveAsFile(await response.Content.ReadAsStringAsync());

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

        private void SaveAsFile(string downloadedRates)
        {
            var path = "../" + _currentRatesFileLocation;

            using StreamWriter outputFile = File.CreateText(Path.Combine(path, "CurrentRates.xml"));
            outputFile.WriteLine(downloadedRates);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}