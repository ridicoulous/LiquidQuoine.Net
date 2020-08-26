using LiquidQuoine.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiquidQuoine.Net.Interfaces
{
    public interface ILiquidQuoineSocketClient
    {

        void SubscribeToOrderBookSide(string symbol, OrderSide side, Action<List<LiquidQuoineOrderBookEntry>, OrderSide, string> onData);
        void SubscribeToUserExecutions(string symbol, Action<LiquidQuoineExecution,string> onData);
        void SubscribeToExecutions(string symbol, Action<LiquidQuoineExecution, string> onData);
        void SubscribeToUserOrdersUpdate(Action<LiquidQuoinePlacedOrder> onData, string[] fundingCurrensies = null);

    }
}
