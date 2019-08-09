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
    public class LiquidQouineAccountCurrencyBalance
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("balance"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Balance { get; set; }

        [JsonProperty("free_balance"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal FreeBalance { get; set; }

        [JsonProperty("reserved_balance"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ReservedBalance { get; set; }

        [JsonProperty("pnl"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Pnl { get; set; }

        [JsonProperty("margin"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Margin { get; set; }

        [JsonProperty("maintenance_margin"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MaintenanceMargin { get; set; }

    }
}
