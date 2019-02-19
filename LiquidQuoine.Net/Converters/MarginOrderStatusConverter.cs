using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class MarginOrderStatusConverter : BaseConverter<MaringOrderStatus>
    {
        public MarginOrderStatusConverter() : this(false) { }
        public MarginOrderStatusConverter(bool quotes) : base(quotes) { }
        protected override Dictionary<MaringOrderStatus, string> Mapping => new Dictionary<MaringOrderStatus, string>
        {
            { MaringOrderStatus.Closed, "closed" },
            { MaringOrderStatus.Opened, "opened" },
        };
    }
}
