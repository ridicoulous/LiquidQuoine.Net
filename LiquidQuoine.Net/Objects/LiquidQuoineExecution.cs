using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoineExecution : LiquidQuoineBase
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

    }
}
/*
 {"event":"created",
 "data":"{\"id+\":99214134,\"quantity+\":\"10.0\",\"price+\":\"0.0010742\",\"taker_side+\":\"sell\",\"created_at+\":1552751727,\"my_side+\":\"buy\",\"pnl\":null,
 \"order_id+\":882566846,\"target\":\"spot\"}","channel":"executions_641444_cash_qasheth"}
     
     */
