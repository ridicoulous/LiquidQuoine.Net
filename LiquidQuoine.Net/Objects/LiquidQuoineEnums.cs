namespace LiquidQuoine.Net.Objects
{
    public enum OrderType
    {
        Limit,
        Market,
        MarketWithRange
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
}
