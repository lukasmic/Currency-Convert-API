using Currency_Convert_API.Application;
using Currency_Convert_API.Features;
using CurrencyConvert.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Categories;

namespace Currency_Convert_API.API.Tests
{
    [UnitTest]
    public class CurrencyRateControllerTests
    {
        private readonly ICurrencyRateHandler _handler;
        private readonly CurrencyRateController _controller;

        public CurrencyRateControllerTests()
        {
            var _repository = new InlineRepository();
            _handler = new CurrencyRateHandler(_repository);
            _controller = new CurrencyRateController(_handler);
        }

        [Theory]
        [MemberData(nameof(CURRENCY_RATES))]
        public void GetConvertedCurrency_GivenValidRequestParameters_ReturnsResponse(string currency, string targetCurrency, double amount, double expectedAmount)
        {
            var result = _controller.GetConvertedCurrency(currency, targetCurrency, amount);

            Assert.Equal(expectedAmount, result.Value);
        }

        [Fact]
        public void GetConvertedCurrency_GivenInvalidRequestParameters_ReturnsNotFound()
        {
            var result = _controller.GetConvertedCurrency("EUR", "INVALID", 1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCurrencies_ReturnsResponse()
        {
            var result = _controller.GetCurrencies();

            var expectedResult = new object[] { "EUR", "USD", "JPY", "BGN" };

            Assert.Equal(expectedResult, result);
        }

        public static readonly IEnumerable<object[]> CURRENCY_RATES = new List<object[]>
        {
            new object[] { "EUR", "JPY", 1, 129.98 },
            new object[] { "EUR", "EUR", 1, 1 },
            new object[] { "EUR", "USD", 2, 2.4132 },
            new object[] { "EUR", "BGN", 1, 1.9558 }
        };
    }
}
