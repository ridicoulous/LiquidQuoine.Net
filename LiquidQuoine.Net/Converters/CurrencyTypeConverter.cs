using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class CurrencyTypeConverter : BaseConverter<CurrencyTypes>
    {
        public CurrencyTypeConverter() : this(false) { }
        public CurrencyTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<CurrencyTypes, string>> Mapping => new List<KeyValuePair<CurrencyTypes, string>>
        {
            new KeyValuePair<CurrencyTypes, string>(CurrencyTypes.Crypto, "crypto"),
            new KeyValuePair<CurrencyTypes, string>(CurrencyTypes.Fiat, "fiat"),
        
        };
    }
}
