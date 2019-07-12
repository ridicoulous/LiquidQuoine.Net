using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineClientOptions : RestClientOptions
    {
        public LiquidQuoineClientOptions()
        {
            BaseAddress = "https://api.liquid.com";            
        }
    }
}
