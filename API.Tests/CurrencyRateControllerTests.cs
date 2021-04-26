using Currency_Convert_API.Entities;
using Currency_Convert_API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Currency_Convert_API.API.Tests
{
    [UnitTest]
    public class CurrencyRateControllerTests
    {
        private readonly InlineRepository _repository;
        private readonly CurrencyRateController _controller;

        public CurrencyRateControllerTests()
        {
            _repository = new InlineRepository();
            _controller = new CurrencyRateController(_repository);
        }

        [Theory]
        [MemberData(nameof(CURRENCY_RATES))]
        public void GetConvertedCurrency_GivenValidRequestParameters_ReturnsResponse(string currency, string targetCurrency, double amount, double expectedAmount)
        {
            var result = _controller.GetConvertedCurrency(currency, targetCurrency, amount);

            Assert.Equal(expectedAmount, result.Value);
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
