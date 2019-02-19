using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Converters
{
    public class OrderSideConverter : BaseConverter<OrderSide>
    {
        public OrderSideConverter() : this(false) { }
        public OrderSideConverter(bool quotes) : base(quotes) { }

        protected override Dictionary<OrderSide, string> Mapping => new Dictionary<OrderSide, string>
        {
            { OrderSide.Buy, "buy" },
            { OrderSide.Sell, "sell" }

        };
    }
}
