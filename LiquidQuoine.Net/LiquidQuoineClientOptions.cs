using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineClientOptions : RestClientOptions
    {
        public LiquidQuoineClientOptions():base("https://api.liquid.com")
        {         
        }
    }
}
