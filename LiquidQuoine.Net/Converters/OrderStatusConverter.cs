using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{

    public class OrderStatusConverter : BaseConverter<OrderStatus>
    {
        public OrderStatusConverter() : this(false) { }
        public OrderStatusConverter(bool quotes) : base(quotes) { }


        protected override List<KeyValuePair<OrderStatus, string>> Mapping => new List<KeyValuePair<OrderStatus, string>>
        {
            new KeyValuePair<OrderStatus, string>(OrderStatus.Canceled, "cancelled"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Filled, "filled"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.Live, "live"),
            new KeyValuePair<OrderStatus, string>(OrderStatus.PartiallyFilled, "partially_filled")

        };
    }
}
