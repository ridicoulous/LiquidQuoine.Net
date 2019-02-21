using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Sockets;
using LiquidQuoine.Net.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects.SocketObjects
{
    public class LiquidSocketRequest : SocketRequest
    {
        public LiquidSocketRequest()
        {
            Signed = false;
        }
        [JsonProperty("event"), JsonConverter(typeof(PusherEventConverter))]
        public PusherEvent Event { get; set; }
    }

    public class LiquidQuoineSubcribeUpdate<T> : LiquidSocketRequest
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonOptionalProperty, JsonProperty("channel")]
        public virtual string Channel { get; set; }
    }

    public class LiquidQuoineSubscribeResponse : LiquidQuoineSubcribeUpdate<object>
    {
        [JsonIgnore]
        public bool IsSuccessful => Event == PusherEvent.SubscribtionSucceeded;
        [JsonIgnore]
        public string ErrorMessage => Data.ToString();
        [JsonIgnore]
        public int ErrorCode => -1;
    }
    /// <summary>
    /// Instatiate and serialize this class to send subsrcibe/uncubscribe request to pusher channel
    /// </summary>
    public class LiquidQuoineSubcribeRequest : LiquidQuoineSubcribeUpdate<Dictionary<string, string>>
    {
        public LiquidQuoineSubcribeRequest(string channel, bool subscribe = true)
        {
            Event = subscribe ? PusherEvent.Subscribe : PusherEvent.Unsubscribe;
            Data = new Dictionary<string, string>() { { "channel", channel } };            
        }
        [JsonIgnore]
        public override string Channel { get; set; }

    }
}

