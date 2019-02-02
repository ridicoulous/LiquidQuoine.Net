using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Objects
{
    public partial class LiquidQuoineOrderBook
    {
        [JsonProperty("buy_price_levels")]
        public List<List<decimal>> BuyPriceLevels { get; set; }

        [JsonProperty("sell_price_levels")]
        public List<List<decimal>> SellPriceLevels { get; set; }
    }
}
