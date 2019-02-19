using LiquidQuoine.Net;
using LiquidQuoine.Net.Objects;
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
            ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("816685", "TONiE8xV1G2p4/UNVrhamIzTiNvrmZSLtL+dIJ1+CrZKBKIaOmuJIG1JrjpXGxkNc0Xl3n1MZsoxQqnJVs0pKA==")
        });

        [Fact]
        public void Should_Buy_10_Qash_By_Eth()
        {
            var my = client.GetMyExecutions(51);
            //var result2 = client.PlaceOrder(51, OrderSide.Sell, OrderType., 10.00m);

            Assert.True(my.Success);
            
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
