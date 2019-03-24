using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class MarginOrderSideConverter : BaseConverter<MaringOrderSide>
    {
        public MarginOrderSideConverter() : this(false) { }
        public MarginOrderSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<MaringOrderSide, string>> Mapping => new List<KeyValuePair<MaringOrderSide, string>>
        {
            new KeyValuePair<MaringOrderSide, string>(MaringOrderSide.Long, "long"),
            new KeyValuePair<MaringOrderSide, string>(MaringOrderSide.Short, "short")

        };
    }
}
