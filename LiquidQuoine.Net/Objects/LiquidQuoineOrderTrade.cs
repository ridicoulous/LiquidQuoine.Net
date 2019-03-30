using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoineOrderTrade
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at"), JsonConverter(typeof(OrderTradeDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at"), JsonConverter(typeof(OrderTradeDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("currency_pair_code")]
        public string CurrencyPairCode { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(MarginOrderStatusConverter))]
        public MaringOrderStatus Status { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(MarginOrderSideConverter))]
        public MaringOrderSide Side { get; set; }

        [JsonProperty("margin_used"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarginUsed { get; set; }

        [JsonProperty("open_quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal OpenQuantity { get; set; }

        [JsonProperty("close_quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal CloseQuantity { get; set; }

        [JsonProperty("quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Quantity { get; set; }

        [JsonProperty("leverage_level")]
        public LeverageLevel LeverageLevel { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("open_price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal OpenPrice { get; set; }

        [JsonProperty("close_price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ClosePrice { get; set; }

        [JsonProperty("trader_id")]
        public long TraderId { get; set; }

        [JsonProperty("open_pnl"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal OpenPnl { get; set; }

        [JsonProperty("close_pnl"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ClosePnl { get; set; }

        [JsonProperty("pnl"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Pnl { get; set; }

        [JsonProperty("stop_loss"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal StopLoss { get; set; }

        [JsonProperty("take_profit"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal TakeProfit { get; set; }

        [JsonProperty("funding_currency")]
        public string FundingCurrency { get; set; }
        [JsonProperty("close_fee"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal CloseFee { get; set; }

        [JsonProperty("total_interest"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal TotalInterest { get; set; }

        [JsonProperty("daily_interest"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal DailyInterest { get; set; }
    }
}
