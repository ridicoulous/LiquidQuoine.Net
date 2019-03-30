using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Converters
{
    public class OrderTypeConverter : BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(false) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "limit"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "market"),
            new KeyValuePair<OrderType, string>(OrderType.MarketWithRange, "market_with_range"),
            new KeyValuePair<OrderType, string>(OrderType.Stop, "stop"),

        };
    }
}
