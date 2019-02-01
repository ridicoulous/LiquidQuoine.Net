using LiquidQuoine.Net;
using System.Linq;
using Xunit;

namespace LuqidExchange.Net.Tests
{
    public class AccountTests
    {
        LiquidQuoineClient client = new LiquidQuoineClient(new LiquidQuoineClientOptions() { ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("786233", "YNDLdz8Se2XOJixAm8TcYSdOuzqs+JDxu+JZVr5NNZBeWkr/D174smXvzuNnZfIhhwFxotPm+vLux5LndV4wuw==") });

        [Fact]
        public void ShouldReturnBalancesList()
        {
          //  var client = new LiquidQuoineClient(new LiquidQuoineClientOptions() { ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("786233", "YNDLdz8Se2XOJixAm8TcYSdOuzqs+JDxu+JZVr5NNZBeWkr/D174smXvzuNnZfIhhwFxotPm+vLux5LndV4wuw==") });
            var result = client.GetAccountsBalances();
            Assert.True(result.Success);
            Assert.True(result.Data.Any());

        }
        [Fact]
        public void ShouldReturnAllProducts()
        {
            var result = client.GetAllProducts();
            Assert.True(result.Success);
            Assert.True(result.Data.Any());

        }
    }
}
