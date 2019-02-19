using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class MarginOrderSideConverter : BaseConverter<MaringOrderSide>
    {
        public MarginOrderSideConverter() : this(false) { }
        public MarginOrderSideConverter(bool quotes) : base(quotes) { }
        protected override Dictionary<MaringOrderSide, string> Mapping => new Dictionary<MaringOrderSide, string>
        {
                 { MaringOrderSide.Long, "long" },
            { MaringOrderSide.Short, "short" }
        };
    }
}
