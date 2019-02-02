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

        [JsonProperty("taker_side")]
        public string TakerSide { get; set; }
        [JsonProperty("created_at"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime CreatedAt { get; set; }

    }
}
