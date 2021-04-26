using System;

namespace Currency_Convert_API.Entities
{
    public class CurrencyRate
    {
        public string Currency { get; init; }
        public double ToEuro { get; init; }
    }
}
