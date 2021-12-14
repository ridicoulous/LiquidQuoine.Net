using CryptoExchange.Net.ExchangeInterfaces;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{
//2019.03.24 12:01:47:255 | Warning | Local object has property `margin_used` but was not found in received object of type `LiquidQuoinePlacedOrder`
//2019.03.24 12:01:47:257 | Warning | Local object has property `margin_interest` but was not found in received object of type `LiquidQuoinePlacedOrder`
//2019.03.24 12:01:47:259 | Warning | Local object has property `unwound_trade_leverage_level` but was not found in received object of type `LiquidQuoinePlacedOrder`
//2019.03.24 12:01:47:260 | Warning | Local object has property `executions` but was not found in received object of type `LiquidQuoinePlacedOrder`
    public class LiquidQuoinePlacedOrder : LiquidQuoineBase, ICommonOrder
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
        [JsonProperty("crypto_account_id")]
        public string CryptoAccountId { get; set; }
        [JsonProperty("source_action")]
        public string SourceAction { get; set; }
        [JsonProperty("unwound_trade_id")]
        public string UnwoundTradeId { get; set; }
        [JsonProperty("trade_id")]
        public string TradeId { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("funding_currency")]
        public string FundingCurrency { get; set; }

        /// <summary>
        /// a.k.a symbol name
        /// </summary>
        [JsonProperty("currency_pair_code")]
        public string CurrencyPairCode { get; set; }

        [JsonProperty("order_fee"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal OrderFee { get; set; }

        [JsonProperty("average_price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal AveragePrice { get; set; }

        [JsonProperty("margin_used"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarginUsed { get; set; }

        [JsonProperty("margin_interest"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarginInterest { get; set; }

        [JsonProperty("unwound_trade_leverage_level")]
        public string UnwoundTradeLeverageLevel { get; set; }
        [JsonProperty("executions")]
        public List<LiquidQuoineOrderExecution> Executions { get; set; }

        public string CommonSymbol => CurrencyPairCode;

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => Quantity;

        public IExchangeClient.OrderStatus CommonStatus => Status switch
         {
             OrderStatus.Canceled => IExchangeClient.OrderStatus.Canceled,
             OrderStatus.Filled => IExchangeClient.OrderStatus.Filled,
             OrderStatus.Live => IExchangeClient.OrderStatus.Active,
             OrderStatus.PartiallyFilled => IExchangeClient.OrderStatus.Active,
             _ => throw new NotImplementedException("Undefined order status")
         };

        public bool IsActive => CommonStatus == IExchangeClient.OrderStatus.Active;

        public IExchangeClient.OrderSide CommonSide => Side switch
        {
            OrderSide.Buy => IExchangeClient.OrderSide.Buy,
            OrderSide.Sell => IExchangeClient.OrderSide.Sell, 
            _ => throw new NotImplementedException("Undefined order side")
        };

        public IExchangeClient.OrderType CommonType => OrderType switch
        {
            OrderType.Limit => IExchangeClient.OrderType.Limit,
            OrderType.Market => IExchangeClient.OrderType.Market,
            _ => IExchangeClient.OrderType.Other
        };

        public DateTime CommonOrderTime => CreatedAt;

        public string CommonId => Id.ToString();
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
