using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class MarginOrderStatusConverter : BaseConverter<MaringOrderStatus>
    {
        public MarginOrderStatusConverter() : this(false) { }
        public MarginOrderStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<MaringOrderStatus, string>> Mapping => new List<KeyValuePair<MaringOrderStatus, string>>
        {
            new KeyValuePair<MaringOrderStatus, string>(MaringOrderStatus.Closed, "closed"),
            new KeyValuePair<MaringOrderStatus, string>(MaringOrderStatus.Opened, "opened")

        };
    }
}
