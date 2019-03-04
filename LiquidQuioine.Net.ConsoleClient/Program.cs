using LiquidQuoine.Net;
using LiquidQuoine.Net.Objects;
using LiquidQuoine.Net.Objects.Socket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiquidQuioine.Net.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var _client = new LiquidQuoineClient(new LiquidQuoineClientOptions() { });
                _client.GetAllProducts();

                LiquidQuoineSocketClient _socketclient = new LiquidQuoineSocketClient(new LiquidQuoineSocketClientOptions()
                {
                    LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug
                    //ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("", "")
                });
                Console.WriteLine("subscrbng");
                _socketclient.SubscribeToOrderBookUpdates("QASHETH", OrderSide.Sell, (book,side) => Catch(book,side));
                _socketclient.SubscribeToOrderBookUpdates("QASHETH", OrderSide.Buy, (book, side) => Catch(book, side));


                Console.WriteLine("asdasdasd");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();

            }
        }
        private static void Catch(List<LiquidQuoineOrderBookEntry> events, OrderSide side)
        {
            var e = events;
            Console.WriteLine(side);
            Console.WriteLine(events[0].Price+ ":"+ events[0].Amount);
        }
    }
}
