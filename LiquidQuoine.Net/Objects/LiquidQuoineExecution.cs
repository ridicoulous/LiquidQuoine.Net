﻿using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoineExecution : LiquidQuoineBase, ICommonRecentTrade
    {
        [JsonProperty("quantity"),JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Quantity { get; set; }
        [JsonProperty("price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }
        [JsonProperty("taker_side"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide TakerSide { get; set; }
        [JsonProperty("my_side"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide MySide { get; set; }
        [JsonProperty("order_id")]
        public long MyOrderId { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("pnl")]
        public string Pnl { get; set; }

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => Quantity;

        public DateTime CommonTradeTime => CreatedAt;
    }
}
