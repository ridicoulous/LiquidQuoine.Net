using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoineFiatAccount
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("balance"),JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Balance { get; set; }

        [JsonProperty("pusher_channel")]
        public string PusherChannel { get; set; }

        [JsonProperty("lowest_offer_interest_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal LowestOfferInterestRate { get; set; }

        [JsonProperty("highest_offer_interest_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal HighestOfferInterestRate { get; set; }

        [JsonProperty("exchange_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ExchangeRate { get; set; }

        [JsonProperty("currency_type"), JsonConverter(typeof(CurrencyTypeConverter))]
        public CurrencyTypes CurrencyType { get; set; }

        [JsonProperty("margin"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Margin { get; set; }

        [JsonProperty("free_margin"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal FreeMargin { get; set; }
    }
}
