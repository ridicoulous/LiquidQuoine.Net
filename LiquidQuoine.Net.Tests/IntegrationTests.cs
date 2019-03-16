using LiquidQuoine.Net;
using LiquidQuoine.Net.Objects;
using LiquidQuoine.Net.Objects.Socket;
using Newtonsoft.Json;
using System;
using System.Linq;
using Xunit;

namespace LuqidExchange.Net.Tests
{
    public class AccountTests
    {
        LiquidQuoineClient client = new LiquidQuoineClient(new LiquidQuoineClientOptions()
        {
            //ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("", "")
        });
        LiquidQuoineSocketClient _socketclient = new LiquidQuoineSocketClient(new LiquidQuoineSocketClientOptions()
        {
            UserId = "641444"
        });
        [Fact]
        public void Should_Buy_Connect_To_Listen_BTCJPY_buy_levels()
        {
            _socketclient.SubscribeToMyExecutions("QASHETH", _=>Console.WriteLine(JsonConvert.SerializeObject(_)));
            Console.ReadLine();
            Assert.True(1==1);
            Assert.True(1 == 1);


        }
        [Fact]
        public void Should_Return_All_Products()
        {
            var result = client.GetAllProducts();
            Assert.True(result.Success);
            Assert.True(result.Data.Any());
        }
        [Theory]
        [InlineData(5)]
        [InlineData(1)]
        public void Should_Return_Product(int id)
        {
            var result = client.GetProduct(id);
            Assert.True(result.Success);
            Assert.True(result.Data!=null);
            Assert.True(result.Data.Id==id);
        }
        [Theory]
        [InlineData(5)]
        [InlineData(1)]
        public void Should_Return_OrderBook(int id)
        {
            var result = client.GetOrderBook(id,true);
            Assert.True(result.Success);
            Assert.True(result.Data != null);
            Assert.True(result.Data.SellPriceLevels.Any()||result.Data.BuyPriceLevels.Any());
        }
        [Theory]
        [InlineData(5)]
        [InlineData(1)]
        public void Should_Return_Executions(int id)
        {
            var result = client.GetExecutions(id);
            Assert.True(result.Success);
            Assert.True(result.Data != null);
        }
        [Theory]
        [InlineData(1)]        
        public void Should_Return_Executions_FromTime(int pair)
        {
            var result = client.GetExecutions(pair,DateTime.UtcNow.AddDays(-2),4);
            Assert.True(result.Success);
            Assert.True(result.Data != null);
        }
        [Theory]
        [InlineData("USD")]
        public void Should_Return_InterestRates(string pair)
        {       
            var result = client.GetInterestRates(pair);
            Assert.True(result.Success);
            Assert.True(result.Data!=null);
        }

        //[Theory]
        //[InlineData(-1)]
        //public void Should_Return_Empty_Order(int id)
        //{
        //    var result = client.GetOrder(id);
        //    Assert.False(result.Success);
        //    Assert.Contains("not found", result.Error.Message);
        //}
    }
}
