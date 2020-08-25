using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClientOptions : SocketClientOptions
    {       
        public string UserId { get; set; }
        public string PushherAppId { get; set; }        

        public LiquidQuoineSocketClientOptions(string userId, string pusherId= "LiquidTapClient") :base("wss://tap.liquid.com")
        {
            UserId = userId;
            PushherAppId = pusherId;
        }

    
    }
}
