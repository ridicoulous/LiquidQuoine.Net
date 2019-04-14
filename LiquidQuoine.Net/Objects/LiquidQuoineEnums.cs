namespace LiquidQuoine.Net.Objects
{
    public enum OrderType
    {
        Limit,
        Market,
        MarketWithRange,
        Stop,
        TrailingStop
    }
    public enum OrderSide
    {
        Buy,
        Sell
    }
    /// <summary>
    /// For margin orders
    /// </summary>
    public enum OrderDirection
    {
        OneDirection, TwoDirection, Netout
    }
    public enum OrderStatus
    {
        Live,
        Filled,
        PartiallyFilled,
        Canceled
    }
    public enum LeverageLevel
    {
        Level2=2,
        Level4=4,
        Level5=5,
        Level10=10,
        Level25=25
    }
    public enum MaringOrderSide
    {
        Long,
        Short
    }
    public enum MaringOrderStatus
    {
        Open,
        Closed
    }

    public enum CurrencyTypes
    {
        Crypto, Fiat
    }
    public enum PusherEvent
    {
        Created,
        Updated,
        Subscribe,
        Unsubscribe,
        ConnectionEstablished,
        SubscribtionSucceeded,
        Ping,
        Pong,     
        OrdersUpdated
    }
}
