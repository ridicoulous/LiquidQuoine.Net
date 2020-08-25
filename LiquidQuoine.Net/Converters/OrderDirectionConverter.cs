using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class OrderDirectionConverter : BaseConverter<OrderDirection>
    {
        public OrderDirectionConverter() : this(false) { }
        public OrderDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderDirection, string>> Mapping => new List<KeyValuePair<OrderDirection, string>>
        {
            new KeyValuePair<OrderDirection, string>(OrderDirection.OneDirection, "one_direction"),
            new KeyValuePair<OrderDirection, string>(OrderDirection.TwoDirection, "two_direction"),
            new KeyValuePair<OrderDirection, string>(OrderDirection.Netout, "netout")

        };
    }
}
