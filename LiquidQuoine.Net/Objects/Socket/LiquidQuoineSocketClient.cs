using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using LiquidQuoine.Net.Converters;
using LiquidQuoine.Net.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PusherClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClient : SocketClient, ILiquidQuoineSocketClient
    {
        private Pusher _pusherClient;
        /*

12:49:49.220
TODO: {"event":"pusher:subscribe","data":{"channel":"product_51_resolution_3600_tickers"}}	
{"event":"pusher:subscribe","data":{"channel":"price_ladders_cash_btcusd_buy"}}
4	
*/

        /// <summary>
        /// need to send user id, eg 651514
        /// </summary>
        private const string UserInfoChannel = "user_{}";
        /// <summary>
        /// need to send pair code e.g. ethusd and pair id, e.g. 27
        /// </summary>
        private const string MarketInfoChannel = "product_cash_{}_{}";
        /// <summary>
        /// Pair code e.g. ethusd
        /// </summary>
        private const string AllExecutionsChannel = "executions_cash_{}";
        /// <summary>
        /// User id, eg 651514 and pair code e.g. ethusd
        /// </summary>
        private const string UserExecutionsChannel = "executions_{}_cash_{}"; 
        /// <summary>
        /// User id, eg 651514 and pair ticker e.g. eth
        /// </summary>
        private const string UserCurrencyUpdatesChannel = "user_{}_account_{}";
        /// <summary>
        /// need to send pair code e.g. ethusd and pair id, e.g. 27
        /// </summary>
        private const string OrderBookSideChannel = "price_ladders_cash_{}_{}";
        private readonly string _currentUserId;
        private TimeSpan socketResponseTimeout = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClient(LiquidQuoineSocketClientOptions options) : base(options, null)
        {
            _currentUserId = options.UserId;
            Configure(options);
            log.Level = LogVerbosity.Debug;
            _pusherClient = new Pusher(options.PushherAppId, new PusherOptions() { ProtocolNumber = 7, Version = "4.4.0",Endpoint = options.BaseAddress });
            _pusherClient.Connect();
        }

        public void SubscribeToOrderBookSide(string symbol, OrderSide side, Action<List<LiquidQuoineOrderBookEntry>, OrderSide, string> onData)
        {
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(OrderBookSideChannel, symbol.ToLower(), JsonConvert.SerializeObject(side, new OrderSideConverter())));
            _myChannel.Bind("updated", (dynamic data) =>
            {
                string t = Convert.ToString(data);
                List<LiquidQuoineOrderBookEntry> deserialized = Deserialize<List<LiquidQuoineOrderBookEntry>>(t).Data;
                onData(deserialized, side, symbol);
            });
        }

        public void SubscribeToUserExecutions(string symbol, Action<LiquidQuoineExecution,string> onData, string userId = null)
        {
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(UserExecutionsChannel, userId ?? _currentUserId, symbol));            
            _myChannel.Bind("created", (dynamic data) =>
            {
                string t = Convert.ToString(data);
                LiquidQuoineExecution deserialized = Deserialize<LiquidQuoineExecution>(t).Data;
                onData(deserialized,symbol);
            });
        }
        public void SubscribeToExecutions(string symbol, Action<LiquidQuoineExecution, string> onData)
        {
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(AllExecutionsChannel, symbol.ToLower()));
            _myChannel.Bind("created", (dynamic data) =>
            {
                string t = Convert.ToString(data);
                LiquidQuoineExecution deserialized = Deserialize<LiquidQuoineExecution>(t).Data;
                onData(deserialized, symbol);
            });
        }



        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            throw new NotImplementedException();
        }

        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            throw new NotImplementedException();
        }

        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            throw new NotImplementedException();
        }

        protected override Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            throw new NotImplementedException();
        }
    }
}