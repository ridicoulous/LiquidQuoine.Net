using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClientOptions : SocketClientOptions
    {
        /// <summary>
        /// The base address for the authenticated websocket
        /// </summary>
        public string BaseAddressAuthenticated { get; set; } = "wss://ws.pusherapp.com/app/2ff981bb060680b5ce97?protocol=7&client=js&version=4.4.0&flash=false";

        /// <summary>
        /// The timeout for socket responses
        /// </summary>
        public TimeSpan SocketResponseTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClientOptions()
        {
            BaseAddress = "wss://ws.pusherapp.com/app/2ff981bb060680b5ce97?protocol=7&client=js&version=4.4.0&flash=false";                
        }

        public LiquidQuoineSocketClientOptions Copy()
        {
            var copy = Copy<LiquidQuoineSocketClientOptions>();
            copy.BaseAddressAuthenticated = BaseAddressAuthenticated;
            copy.SocketResponseTimeout = SocketResponseTimeout;
            return copy;
        }
    }
}
