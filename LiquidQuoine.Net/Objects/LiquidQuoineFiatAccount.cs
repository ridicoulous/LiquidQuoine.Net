using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoineAccountBase
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("balance"),JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Balance { get; set; }

        [JsonProperty("reserved_balance"),JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ReservedBalance { get; set; }

        [JsonProperty("pusher_channel")]
        public string PusherChannel { get; set; }

        [JsonProperty("lowest_offer_interest_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal LowestOfferInterestRate { get; set; }

        [JsonProperty("highest_offer_interest_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal HighestOfferInterestRate { get; set; }

        [JsonProperty("currency_type"), JsonConverter(typeof(CurrencyTypeConverter))]
        public CurrencyTypes CurrencyType { get; set; }
    }

    public class LiquidQuoineFiatAccount : LiquidQuoineAccountBase
    {
        [JsonProperty("address")]
        public decimal Address { get; set; }

        [JsonProperty("minimum_withdraw"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MinimumWithdraw { get; set; }
    }

    public class LiquidQuoineCryptoAccount : LiquidQuoineAccountBase
    {
        [JsonProperty("exchange_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ExchangeRate { get; set; }
    }
}
