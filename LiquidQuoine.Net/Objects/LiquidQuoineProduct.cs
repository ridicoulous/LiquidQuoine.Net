using CryptoExchange.Net.ExchangeInterfaces;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;

namespace LiquidQuoine.Net.Objects
{

    public partial class LiquidQuoineProduct : ICommonSymbol, ICommonTicker
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market_ask"),JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarketAsk { get; set; }

        [JsonProperty("market_bid"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MarketBid { get; set; }

        [JsonProperty("indicator")]
        public int? Indicator { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currency_pair_code")]
        public string CurrencyPairCode { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("fiat_minimum_withdraw"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal FiatMinimumWithdraw { get; set; }

        [JsonProperty("pusher_channel")]
        public string PusherChannel { get; set; }

        [JsonProperty("taker_fee"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal TakerFee { get; set; }

        [JsonProperty("maker_fee"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal MakerFee { get; set; }

        [JsonProperty("low_market_bid"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal LowMarketBid { get; set; }

        [JsonProperty("high_market_ask"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal HighMarketAsk { get; set; }

        [JsonProperty("volume_24h"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Volume24H { get; set; }

        [JsonProperty("last_price_24h"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal LastPrice24H { get; set; }

        [JsonProperty("last_traded_price"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal LastTradedPrice { get; set; }

        [JsonProperty("last_traded_quantity"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal LastTradedQuantity { get; set; }

        [JsonProperty("quoted_currency")]
        public string QuotedCurrency { get; set; }

        [JsonProperty("base_currency")]
        public string BaseCurrency { get; set; }

        [JsonProperty("exchange_rate"), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ExchangeRate { get; set; }

        [JsonProperty("disabled")]
        public bool IsDisabled { get; set; }

        public string CommonName => CurrencyPairCode;

        public decimal CommonMinimumTradeSize => 0m;

        public string CommonSymbol => CurrencyPairCode;

        public decimal CommonHigh => HighMarketAsk;

        public decimal CommonLow => LowMarketBid;

        public decimal CommonVolume => Volume24H;
    }
}