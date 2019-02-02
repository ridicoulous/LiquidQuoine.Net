using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{
    public partial class LiquidQuoineInterestRate
    {
        [JsonProperty("bids")]
        public List<LiquidQuoineOrderBookEntry> Bids { get; set; }
        [JsonProperty("asks")]
        public List<LiquidQuoineOrderBookEntry> Asks { get; set; }


    }
    [JsonConverter(typeof(ArrayConverter))]
    public class LiquidQuoineInterestRateEntry
    {

        [ArrayProperty(0), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Rate { get; set; }
        [ArrayProperty(1), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Amount { get; set; }
    }
}
