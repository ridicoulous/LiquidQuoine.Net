using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using LiquidQuoine.Net.Converters;
using LiquidQuoine.Net.Interfaces;
using LiquidQuoine.Net.Objects.SocketObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClient : SocketClient, ILiquidQuoineSocketClient
    {
        /*
         event":"pusher:subscribe","data":{"channel":"product_cash_qasheth_51"}}	73	



12:49:45.748
{"event":"pusher:subscribe","data":{"channel":"price_ladders_cash_qasheth_buy"}}	80	
12:49:47.770
{"event":"pusher:subscribe","data":{"channel":"price_ladders_cash_qasheth_sell"}}	81	
12:49:47.770
{"event":"pusher:subscribe","data":{"channel":"user_634841"}}	61	
12:49:47.779
{"event":"pusher:subscribe","data":{"channel":"executions_cash_qasheth"}}	73	
12:49:48.662
{"event":"pusher:subscribe","data":{"channel":"executions_634841_cash_qasheth"}}	80	
12:49:48.666
{"event":"pusher:subscribe","data":{"channel":"user_634841_account_eth"}}	73	
12:49:49.220
{"event":"pusher:subscribe","data":{"channel":"product_51_resolution_3600_tickers"}}	84	
12:49:50.965
{"event":"pusher:unsubscribe","data":{"channel":"product_51_resolution_3600_tickers"}}	86	
12:49:51.553
{"event":"pusher:subscribe","data":{"channel":"product_51_resolution_3600_tickers"}}	84	
12:49:52.027
*/
        private readonly string _currentUserId;
        private TimeSpan socketResponseTimeout = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClient(LiquidQuoineSocketClientOptions options) : base(options, null)
        {
            _currentUserId = options.UserId;
            Configure(options);
            log.Level = LogVerbosity.Debug;   
        }
        #region private
      
        public CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, OrderSide side, Action<List<LiquidQuoineOrderBookEntry>,OrderSide,string> onData) => SubscribeToOrderBookUpdatesAsync(symbol,side, onData).Result;

        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, OrderSide side, Action<List<LiquidQuoineOrderBookEntry>,OrderSide, string> onData)
        {
            var request = new LiquidQuoineSubcribeRequest($"price_ladders_cash_{symbol.ToLower()}_{JsonConvert.SerializeObject(side,new OrderSideConverter())}");
            var internalHandler = new Action<LiquidQuoineSubcribeUpdate<List<LiquidQuoineOrderBookEntry>>>(data =>
            {               
                onData(data.Data,side,symbol);
            });
            return await Subscribe(request, internalHandler).ConfigureAwait(false);
        }
        public CallResult<UpdateSubscription> SubscribeToMyExecutions(string symbol, Action<LiquidQuoineExecution,string> onData) => SubscribeToMyExecutionsUpdatesAsync(symbol, onData).Result;
        public async Task<CallResult<UpdateSubscription>> SubscribeToMyExecutionsUpdatesAsync(string symbol,Action<LiquidQuoineExecution,string> onData)
        {
            var request = new LiquidQuoineSubcribeRequest($"executions_{_currentUserId}_cash_{symbol.ToLower()}");
            var internalHandler = new Action<LiquidQuoineSubcribeUpdate<LiquidQuoineExecution>>(data =>
            {
                onData(data.Data,symbol);
            });
            return await Subscribe(request, internalHandler).ConfigureAwait(false);
        }

        private async Task<CallResult<UpdateSubscription>> Subscribe<T>(LiquidSocketRequest request, Action<T> onData) where T : class
        {
            var connectResult = await CreateAndConnectSocket(request, onData).ConfigureAwait(false);
            if (!connectResult.Success)
                return new CallResult<UpdateSubscription>(null, connectResult.Error);

            var subscription = connectResult.Data;
            //subscription.AddEvent(JsonConvert.SerializeObject(PusherEvent.ConnectionEstablished, new PusherEventConverter()));
            //subscription.AddEvent(JsonConvert.SerializeObject(PusherEvent.SubscribtionSucceeded, new PusherEventConverter()));
           // subscription.AddEvent(JsonConvert.SerializeObject(PusherEvent.Updated, new PusherEventConverter()));

            Send(subscription.Socket, request);

            subscription.Request = request;
            subscription.Socket.ShouldReconnect = true;
            return new CallResult<UpdateSubscription>(new UpdateSubscription(subscription), null);
        }
      
        
        private bool DataHandler<T>(SocketSubscription subscription, JToken data, Action<T> handler) where T : class
        {          
            var desResult = Deserialize<T>(data.ToString().Replace("\"[[", "[[").Replace("]]\"", "]]").Replace("\\", "").Replace("\"{","{").Replace("}\"", "}"), true);
            if (!desResult.Success)
            {
                log.Write(LogVerbosity.Warning, $"Failed to deserialize data: {desResult.Error}. Data: {data}");
                return false;
            }
            handler(desResult.Data);
            subscription.SetEventByName(DataEvent, true, null);
            return true;
        }
        //private bool SubscriptionHandlerV1(SocketSubscription subscription, JToken data)
        //{
        //    var v1Sub = data["subbed"] != null;
        //    var v1Error = data["status"] != null && (string)data["status"] == "error";
        //    if (v1Sub || v1Error)
        //    {
        //        var subResponse = Deserialize<HuobiSubscribeResponse>(data, false);
        //        if (!subResponse.Success)
        //        {
        //            log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Error);
        //            subscription.SetEventByName(SubscriptionEvent, false, subResponse.Error);
        //            return true;
        //        }

        //        if (!subResponse.Data.IsSuccessful)
        //        {
        //            log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Data.ErrorMessage);
        //            subscription.SetEventByName(SubscriptionEvent, false, new ServerError($"{subResponse.Data.ErrorCode}, {subResponse.Data.ErrorMessage}"));
        //            return true;
        //        }

        //        log.Write(LogVerbosity.Debug, "Subscription completed");
        //        subscription.SetEventByName(SubscriptionEvent, true, null);
        //        return true;
        //    }

        //    return false;
        //}

        //private bool SubscriptionHandlerV2(SocketSubscription subscription, JToken data)
        //{
        //    var v2Sub = (string)data["op"] == "sub";
        //    if (!v2Sub)
        //        return false;

        //    var subResponse = Deserialize<HuobiSocketAuthResponse>(data, false);
        //    if (!subResponse.Success)
        //    {
        //        log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Error);
        //        subscription.SetEventByName(SubscriptionEvent, false, subResponse.Error);
        //        return true;
        //    }

        //    if (!subResponse.Data.IsSuccessful)
        //    {
        //        log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Data.ErrorMessage);
        //        subscription.SetEventByName(SubscriptionEvent, false, new ServerError(subResponse.Data.ErrorCode, subResponse.Data.ErrorMessage));
        //        return true;
        //    }

        //    log.Write(LogVerbosity.Debug, "Subscription completed");
        //    subscription.SetEventByName(SubscriptionEvent, true, null);
        //    return true;
        //}

        private async Task<CallResult<SocketSubscription>> CreateAndConnectSocket<T>(LiquidSocketRequest request, Action<T> onMessage) where T:class
        {
            var socket = CreateSocket(BaseAddress);
            var subscription = new SocketSubscription(socket);
            subscription.AddEvent("pusher_internal:subscription_succeeded");
            subscription.MessageHandlers.Add(DataHandlerName, (subs, data) => DataHandler(subs, data, onMessage));
            subscription.MessageHandlers.Add("pusher_internal:subscription_succeeded", (subs, data) => SubscriptionHandler(subs, data));
           // subscription.MessageHandlers.Add(SubscriptionHandlerName, SubscriptionHandler);
            var connectResult = await ConnectSocket(subscription).ConfigureAwait(false);
            if (!connectResult.Success)
                return new CallResult<SocketSubscription>(null, connectResult.Error);
            socket.ShouldReconnect = true;      
            return new CallResult<SocketSubscription>(subscription, null);
        }
        private bool SubscriptionHandler(SocketSubscription subscription, JToken data)
        {
            var check = data["event"] != null;
            log.Write(LogVerbosity.Debug, data.ToString());            
            if (check)
            {
                var subResponse = Deserialize<LiquidQuoineSubscribeResponse>(data, false);
                if (!subResponse.Success)
                {
                    log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Error);
                    subscription.SetEventByName(SubscriptionEvent, false, subResponse.Error);
                    return true;
                }

                if (!subResponse.Data.IsSuccessful)
                {
                    log.Write(LogVerbosity.Warning, "Subscription failed: " + subResponse.Data.ErrorMessage);
                    subscription.SetEventByName(SubscriptionEvent, false, new ServerError($"{subResponse.Data.ErrorCode}, {subResponse.Data.ErrorMessage}"));
                    return true;
                }

                log.Write(LogVerbosity.Debug, "Subscription completed");
                subscription.SetEventByName(SubscriptionEvent, true, null);
                return true;
            }

            return false;
        }

        protected override bool SocketReconnect(SocketSubscription subscription, TimeSpan disconnectedTime)
        {
            var request = (LiquidSocketRequest)subscription.Request;
            if (request.Signed)
            {
                log.Write(LogVerbosity.Info, "ws auth not implemented");
            }

            Send(subscription.Socket, request);

            return subscription.WaitForEvent(SubscriptionEvent, socketResponseTimeout).Result.Success;
        }

        //private static string DecompressData(byte[] byteData)
        //{
        //    using (var decompressedStream = new MemoryStream())
        //    using (var compressedStream = new MemoryStream(byteData))
        //    using (var deflateStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        //    {
        //        deflateStream.CopyTo(decompressedStream);
        //        decompressedStream.Position = 0;

        //        using (var streamReader = new StreamReader(decompressedStream))
        //        {
        //            return streamReader.ReadToEnd();
        //        }
        //    }
        //}
     
    }
    #endregion
}