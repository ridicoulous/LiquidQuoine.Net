using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Converters
{
   
    public class OrderStatusConverter : BaseConverter<OrderStatus>
    {
        public OrderStatusConverter() : this(true) { }
        public OrderStatusConverter(bool quotes) : base(quotes) { }

        protected override Dictionary<OrderStatus, string> Mapping => new Dictionary<OrderStatus, string>
        {
            { OrderStatus.Canceled, "cancelled" },
            { OrderStatus.Filled, "filled" },
            { OrderStatus.Live, "live" },
            { OrderStatus.PartiallyFilled, "partially_filled" }
        };
    }
}
