using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Currency_Convert_API.HostedServices
{
    public class CurrencyRateFetchService : IHostedService
    {
        private readonly ILogger<CurrencyRateFetchService> _logger;
        private int _downloadFrequency;
        private string _fileLocation;

        public CurrencyRateFetchService(IConfiguration configuration, ILogger<CurrencyRateFetchService> logger)
        {
            _logger = logger;
            _downloadFrequency = int.Parse(configuration.GetSection("RatesDownloadFrequencyInMinutes").Value ?? throw new ArgumentNullException("RatesDownloadFrequencyInMinutes was not found in appsettings.json"));
            _fileLocation = configuration.GetSection("RatesFileLocation").Value ?? throw new ArgumentNullException("RatesFileLocation was not found in appsettings.json");
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
            var path = _fileLocation;

            using StreamWriter outputFile = File.CreateText(Path.Combine(path, "eurofxref-daily.xml"));
            outputFile.WriteLine(downloadedRates);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}