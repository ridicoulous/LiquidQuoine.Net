using LiquidQuoine.Net;
using System.Linq;
using Xunit;

namespace LuqidExchange.Net.Tests
{
    public class AccountTests
    {
        LiquidQuoineClient client = new LiquidQuoineClient(new LiquidQuoineClientOptions() { ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("786233", "YNDLdz8Se2XOJixAm8TcYSdOuzqs+JDxu+JZVr5NNZBeWkr/D174smXvzuNnZfIhhwFxotPm+vLux5LndV4wuw==") });

        [Fact]
        public void Should_Return_Balances_List()
        {
          //  var client = new LiquidQuoineClient(new LiquidQuoineClientOptions() { ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("786233", "YNDLdz8Se2XOJixAm8TcYSdOuzqs+JDxu+JZVr5NNZBeWkr/D174smXvzuNnZfIhhwFxotPm+vLux5LndV4wuw==") });
            var result = client.GetAccountsBalances();
            Assert.True(result.Success);
            Assert.True(result.Data.Any());

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
            var result = client.GetOrderBook(id);
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
          //  Assert.True(result.Data.SellPriceLevels.Any() || result.Data.BuyPriceLevels.Any());
        }
    }
}
