using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class TradingTypeConverter : BaseConverter<TradingType>
    {
        public TradingTypeConverter() : this(false) { }
        public TradingTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TradingType, string>> Mapping => new List<KeyValuePair<TradingType, string>>
        {
            new KeyValuePair<TradingType, string>(TradingType.Spot, "spot"),
            new KeyValuePair<TradingType, string>(TradingType.ContractForDifference, "cfd"),
            new KeyValuePair<TradingType, string>(TradingType.Margin, "margin"),
            new KeyValuePair<TradingType, string>(TradingType.Perpetual, "perpetual"),
        };
    }
}
