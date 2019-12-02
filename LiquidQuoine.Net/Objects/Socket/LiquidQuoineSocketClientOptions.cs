using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClientOptions : SocketClientOptions
    {
       
        public string UserId { get; set; }
        public string PushherAppId { get; set; }

        public TimeSpan SocketResponseTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClientOptions(string userId, string pusherId= "2ff981bb060680b5ce97"):base("wss://echo.websocket.org")
        {
            UserId = userId;
            PushherAppId = pusherId;
        }

    
    }
}
