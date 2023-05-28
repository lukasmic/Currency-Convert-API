using CurrencyConvert.Application.CurrentRate;
using CurrencyConvert.Infrastructure.Repository;
using Xunit;
using Xunit.Categories;

namespace CurrencyConvert.Application.Tests
{
    [UnitTest]
    public class CurrentRateHandlerTests
    {
        private readonly CurrentRateHandler handler;

        public CurrentRateHandlerTests()
        {
            handler = new CurrentRateHandler(new InlineRepository());
        }

        [Theory]
        [MemberData(nameof(CURRENCY_RATES))]
        public void GetCurrencyRate_GivenValidRequestParameters_ReturnsResponse(string currency, double expectedAmount)
        {
            var result = handler.GetCurrencyRate(currency);

            Assert.Equal(expectedAmount, result.ToEuro);
        }

        [Fact]
        public void GetCurrencyRate_GivenInvalidRequestParameters_ReturnsNull()
        {
            var result = handler.GetCurrencyRate("INVALID");

            Assert.Null(result);
        }

        [Fact]
        public void GetCurrencyRateNames_ReturnsResponse()
        {
            var result = handler.GetCurrencyRateNames();

            var expectedResult = new object[] { "EUR", "USD", "JPY", "BGN" };

            Assert.Equal(expectedResult, result);
        }

        public static readonly IEnumerable<object[]> CURRENCY_RATES = new List<object[]>
        {
            new object[] { "JPY", 129.98 },
            new object[] { "EUR", 1 },
            new object[] { "BGN", 1.9558 },
            new object[] { "USD", 1.2066 }
        };
    }
}
