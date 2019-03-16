using CryptoExchange.Net.Converters;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Converters
{
    public class PusherEventConverter : BaseConverter<PusherEvent>
    {
        public PusherEventConverter() : this(true) { }
        public PusherEventConverter(bool quotes) : base(quotes) { }
        protected override Dictionary<PusherEvent, string> Mapping => new Dictionary<PusherEvent, string>
        {
            { PusherEvent.ConnectionEstablished, "pusher:connection_established" },
            { PusherEvent.Subscribe, "pusher:subscribe" },
            { PusherEvent.SubscribtionSucceeded, "pusher_internal:subscription_succeeded" },
            { PusherEvent.Unsubscribe, "pusher:unsubscribe" },
            { PusherEvent.Updated, "updated" },
            { PusherEvent.Created, "created" },
            { PusherEvent.Ping, "pusher:ping" },
            { PusherEvent.Pong, "pusher:pong" },
            { PusherEvent.OrdersUpdated, "orders_updated" }

            
        };
    }
}
