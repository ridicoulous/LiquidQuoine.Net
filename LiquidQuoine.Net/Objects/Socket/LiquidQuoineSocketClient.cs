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
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClient : SocketClient, ILiquidQuoineSocketClient
    {
        private readonly Pusher _pusherClient;
        /*
TODO: {"event":"pusher:subscribe","data":{"channel":"product_51_resolution_3600_tickers"}}	
{"event":"pusher:subscribe","data":{"channel":"price_ladders_cash_btcusd_buy"}}
*/
        /// <summary>
        /// need to send pair code e.g. ethusd and pair id, e.g. 27
        /// </summary>
        private const string MarketInfoChannel = "product_cash_{}_{}";
        /// <summary>
        /// Pair code e.g. ethusd
        /// </summary>
        private const string AllExecutionsChannel = "executions_cash_{}";      
        /// <summary>
        /// need to send pair code e.g. ethusd and pair id, e.g. 27
        /// </summary>
        private const string OrderBookSideChannel = "price_ladders_cash_{}_{}";
        /// <summary>
        /// New order appears in ladder Existing order updated in ladder Order is removed from ladder * Displays both bid and ask prices of the selected market. 
        /// params: code, currency_pair_code
        /// </summary>
        private const string UserAccountPriceBookEndpoint = "price_ladders_{}_{}";
        /// <summary>
        /// User’s order is created
        ///User’s order is cancelled
        ///User’s order is filled
        ///Order’s stop-loss or take profit is updated.
        ///params: order dunding currency
        /// </summary>
        private const string UserAccountOrdersEndpoint = "user_account_{}_orders";
        /// <summary>
        /// User’s position is created,        User’s position is closed
        /// params: order dunding currency
        /// </summary>
        private const string UserAccountTradesEndpoint = "user_account_{}_trades";
        /// <summary>
        /// User’s execution is created
        /// params: currency_pair_code
        /// </summary>
        private const string UserAccountExecutionsEndpoint = "user_executions_cash_{}";



        private TimeSpan socketResponseTimeout = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClient(LiquidQuoineSocketClientOptions options) : base("Liquid", options, null)
        {
            authProvider = options.authenticationProvider;
            
            // Configure(options);
            log.Level = LogVerbosity.Debug;
            _pusherClient = new Pusher(options.PushherAppId, new PusherOptions()
            {
                ProtocolNumber = 7,
                Version = "4.4.0",
                Endpoint = "tap.liquid.com",
                Encrypted = true,
                Client = "",
            });
            _pusherClient.ConnectionStateChanged += _pusherClient_ConnectionStateChanged;
            _pusherClient.Connected += _pusherClient_Connected;
            _pusherClient.Connect();
        }

        private void _pusherClient_Connected(object sender)
        {
            log.Write(LogVerbosity.Debug,"Liquid client is connected");
            if (authProvider != null)
            {
                Authenticate();
            }
        }

        private void _pusherClient_ConnectionStateChanged(object sender, ConnectionState state)
        {
            log.Write(LogVerbosity.Debug,$"Socket client {sender} state setted to {state.ToString()}");
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
        /// <summary>
        /// By default when not send user preffered fundingCurrensies, subscribes to each pair for each funding currency ("usd", "btc", "cash", "eth").
        /// </summary>
        /// <param name="onData">action on update</param>
        /// <param name="fundingCurrensies">user preffered funding currencies</param>
        public void SubscribeToUserOrdersUpdate(Action<LiquidQuoinePlacedOrder> onData, string[] fundingCurrensies=null)
        {
            if (authProvider == null)
                throw new Exception("for subscribing to private channels you must provide api credentials");
            if (fundingCurrensies == null || !fundingCurrensies.Any())
            {
                fundingCurrensies = new string[] { "usd", "btc", "cash", "eth" };
            }
            foreach(var fundingCurrency in fundingCurrensies)
            {
                var channel = _pusherClient.Subscribe(FillPathParameter(UserAccountOrdersEndpoint, fundingCurrency));
                channel.Bind("update", (dynamic data) =>
                {
                    string t = Convert.ToString(data);
                    LiquidQuoinePlacedOrder deserialized = Deserialize<LiquidQuoinePlacedOrder>(t,false).Data;
                    onData(deserialized);
                });
            }
        }
        /// <summary>
        /// subscribes to user executions
        /// </summary>
        /// <param name="symbol">currency_pair_code at liquid </param>
        /// <param name="onData"></param>
        public void SubscribeToUserExecutions(string symbol, Action<LiquidQuoineExecution, string> onData)
        {
            if (authProvider == null)
                throw new Exception("for subscribing to private channels you must provide api credentials");
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(UserAccountExecutionsEndpoint, symbol));
            _myChannel.Bind("update", (dynamic data) =>
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

        private void Authenticate()
        {
            if (authProvider == null)
                throw new Exception("You must provide api credentials to subscribing to private streams");

            var p = authProvider.AddAuthenticationToHeaders("/realtime", HttpMethod.Get, new Dictionary<string, object>(), true, PostParameters.InBody, ArrayParametersSerialization.Array);
            var t = JsonConvert.SerializeObject(new { @event= "quoine:auth_request", data= new{ path = "/realtime", headers = new Dictionary<string, string>() { { "X-Quoine-Auth", p["X-Quoine-Auth"] } }} });
            Console.WriteLine(t);
            _pusherClient.Bind("quoine:auth_success", onSuccessAuth);
            _pusherClient.Bind("quoine:auth_failure", onNotSuccessAuth);
            _pusherClient.SendMessage(t); 
        }

        private void onNotSuccessAuth(dynamic obj)
        {
            log.Write(LogVerbosity.Error, "Can not open private stream");

        }

        private void onSuccessAuth(dynamic obj)
        {
            log.Write(LogVerbosity.Debug, "succesfully authenticated to private stream");
        }
    }
}