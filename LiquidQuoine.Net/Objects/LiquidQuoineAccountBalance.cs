using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQouineAccountBalance
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("balance"),  JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Balance { get; set; }

    }
}
