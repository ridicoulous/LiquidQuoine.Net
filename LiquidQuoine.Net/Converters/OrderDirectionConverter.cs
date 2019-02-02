using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class OrderDirectionConverter : BaseConverter<OrderDirection>
    {
        public OrderDirectionConverter() : this(true) { }
        public OrderDirectionConverter(bool quotes) : base(quotes) { }
        protected override Dictionary<OrderDirection, string> Mapping => new Dictionary<OrderDirection, string>
        {
            { OrderDirection.OneDirection, "one_direction" },
            { OrderDirection.TwoDirection, "two_direction" },
            { OrderDirection.Netout, "netout" },
        };
    }
}
