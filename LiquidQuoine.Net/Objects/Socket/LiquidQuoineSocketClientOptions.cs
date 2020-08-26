using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClientOptions : SocketClientOptions
    {       
        public string PushherAppId { get; set; }
        public readonly LiquidQuoineAuthenticationProvider authenticationProvider;

        public LiquidQuoineSocketClientOptions() :base("wss://tap.liquid.com")
        {            
            PushherAppId = "LiquidTapClient";
        }
        public LiquidQuoineSocketClientOptions(string key, string secret) : base("wss://tap.liquid.com")
        {
            authenticationProvider = new LiquidQuoineAuthenticationProvider(new CryptoExchange.Net.Authentication.ApiCredentials(key, secret));
          
            PushherAppId = "LiquidTapClient";
        }
        public LiquidQuoineSocketClientOptions(LiquidQuoineAuthenticationProvider auth) : base("wss://tap.liquid.com")
        {
            authenticationProvider = auth;          
            PushherAppId = "LiquidTapClient";
        }

    }
}
