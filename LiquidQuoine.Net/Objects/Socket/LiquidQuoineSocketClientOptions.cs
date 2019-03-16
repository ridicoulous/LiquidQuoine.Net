using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClientOptions : SocketClientOptions
    {
        /// <summary>
        /// The base address for the authenticated websocket
        /// </summary>
        public string BaseAddressAuthenticated { get; set; } //= "wss://ws.pusherapp.com/app/2ff981bb060680b5ce97?protocol=7&client=js&version=4.4.0&flash=false";
        public string UserId { get; set; }
        /// <summary>
        /// The timeout for socket responses
        /// </summary>
        public TimeSpan SocketResponseTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClientOptions(string userId, string baseAddressAuth=null, string baseAddress = "wss://ws.pusherapp.com/app/2ff981bb060680b5ce97?protocol=7&client=js&version=4.4.0&flash=false")
        {
            BaseAddress = baseAddress;
            UserId = userId;
            BaseAddressAuthenticated = baseAddressAuth ?? baseAddress;
        }

        //public LiquidQuoineSocketClientOptions Copy(string userId=null)
        //{
        //    var copy = Copy<LiquidQuoineSocketClientOptions>(userid);
        //    copy.BaseAddressAuthenticated = BaseAddressAuthenticated;
        //    copy.SocketResponseTimeout = SocketResponseTimeout;
        //    return copy;
        //}
    }
}
