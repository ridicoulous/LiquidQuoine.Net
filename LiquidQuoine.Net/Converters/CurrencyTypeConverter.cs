using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class CurrencyTypeConverter : BaseConverter<CurrencyTypes>
    {
        public CurrencyTypeConverter() : this(true) { }
        public CurrencyTypeConverter(bool quotes) : base(quotes) { }
        protected override Dictionary<CurrencyTypes, string> Mapping => new Dictionary<CurrencyTypes, string>
        {
            { CurrencyTypes.Crypto, "crypto" },
            { CurrencyTypes.Fiat, "fiat" }
        };
    }
}
