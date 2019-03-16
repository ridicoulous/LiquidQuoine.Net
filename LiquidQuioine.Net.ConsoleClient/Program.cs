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

                LiquidQuoineSocketClient _socketclient = new LiquidQuoineSocketClient(new LiquidQuoineSocketClientOptions("")
                {
                    LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug,
                     
                    //ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("", "")
                });
                Console.WriteLine("subscrbng");
                _socketclient.SubscribeToMyExecutions("QASHETH", Catch);



                Console.WriteLine("asdasdasd");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();

            }
        }
        private static void Catch(LiquidQuoineExecution e)
        {
            var eo = e;
            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(eo));
            }
            catch (Exception ex)
            {
                Console.WriteLine("hmmm");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
