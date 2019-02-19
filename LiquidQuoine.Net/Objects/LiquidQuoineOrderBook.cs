using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{
    public partial class LiquidQuoineOrderBook
    {
        /// <summary>
        /// Asks
        /// </summary>
        [JsonProperty("buy_price_levels")]
        public List<LiquidQuoineOrderBookEntry> BuyPriceLevels { get; set; }
        /// <summary>
        /// Bids
        /// </summary>
        [JsonProperty("sell_price_levels")]
        public List<LiquidQuoineOrderBookEntry> SellPriceLevels { get; set; }
    }
    [JsonConverter(typeof(ArrayConverter))]
    public class LiquidQuoineOrderBookEntry
    {

        [ArrayProperty(0), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }
        [ArrayProperty(1), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Amount { get; set; }
    }
}
