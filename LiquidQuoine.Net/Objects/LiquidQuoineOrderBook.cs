using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{
    public class LiquidQuoineOrderBook : ICommonOrderBook
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

        public IEnumerable<ISymbolOrderBookEntry> CommonBids => BuyPriceLevels;

        public IEnumerable<ISymbolOrderBookEntry> CommonAsks => SellPriceLevels;
    }
    [JsonConverter(typeof(ArrayConverter))]
    public class LiquidQuoineOrderBookEntry : ISymbolOrderBookEntry
    {
        [ArrayProperty(0), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Price { get; set; }
        [ArrayProperty(1), JsonConverter(typeof(StringToDecimalConverter))]
        public decimal Amount { get; set; }
        public decimal Quantity { get => Amount; set => Amount = value; }
    }
}
