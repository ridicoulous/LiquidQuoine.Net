using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Converters
{
    public class OrderTypeConverter : BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override Dictionary<OrderType, string> Mapping => new Dictionary<OrderType, string>
        {
            { OrderType.Limit, "limit" },
            { OrderType.Market, "market" },
            { OrderType.MarketWithRange, "market_with_range" },
     
        };
    }
}
