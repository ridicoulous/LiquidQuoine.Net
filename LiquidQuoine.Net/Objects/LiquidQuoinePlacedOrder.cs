using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoinePlacedOrder : LiquidQuoineBase
    {

        [JsonProperty("order_type"), JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }

        [JsonProperty("quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Quantity { get; set; }

        [JsonProperty("disc_quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal DiscQuantity { get; set; }

        [JsonProperty("iceberg_total_quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal IcebergTotalQuantity { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }

        [JsonProperty("filled_quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal FilledQuantity { get; set; }

        [JsonProperty("price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }

        [JsonProperty("leverage_level")]
        public LeverageLevel LeverageLevel { get; set; }

        [JsonProperty("source_exchange")]
        public string SourceExchange { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("funding_currency")]
        public string FundingCurrency { get; set; }

        [JsonProperty("currency_pair_code")]
        public string CurrencyPairCode { get; set; }

        [JsonProperty("order_fee"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal OrderFee { get; set; }

        [JsonProperty("margin_used"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarginUsed { get; set; }

        [JsonProperty("margin_interest"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarginInterest { get; set; }

        [JsonProperty("unwound_trade_leverage_level")]
        public string UnwoundTradeLeverageLevel { get; set; }
        [JsonProperty("executions")]
        public List<LiquidQuoineOrderExecution> Executions { get; set; }
    }

    public class LiquidQuoineOrderExecution : LiquidQuoineBase
    {
        [JsonProperty("quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Quantity { get; set; }
        [JsonProperty("price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }
        [JsonProperty("taker_side"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide TakerSide { get; set; }
        [JsonProperty("my_side"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide MySide { get; set; }

    }
}
