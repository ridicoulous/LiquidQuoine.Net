using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineClientOptions : ClientOptions
    {
        public LiquidQuoineClientOptions()
        {
            BaseAddress = "https://api.liquid.com";
        }
    }
}
