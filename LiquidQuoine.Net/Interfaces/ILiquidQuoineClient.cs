using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using LiquidQuoine.Net.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiquidQuoine.Net.Interfaces
{
    public interface ILiquidQuoineClient
    {
        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);


        RateLimitingBehaviour RateLimitBehaviour { get; }
        IEnumerable<IRateLimiter> RateLimiters { get; }
        string BaseAddress { get; }

        /// <summary>
        /// Adds a rate limiter to the client. There are 2 choices, the <see cref="RateLimiterTotal"/> and the <see cref="RateLimiterPerEndpoint"/>.
        /// </summary>
        /// <param name="limiter">The limiter to add</param>
        void AddRateLimiter(IRateLimiter limiter);

        /// <summary>
        /// Removes all rate limiters from this client
        /// </summary>
        void RemoveRateLimiters();

        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        CallResult<List<LiquidQuoineProduct>> GetAllProducts();
        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        Task<CallResult<List<LiquidQuoineProduct>>> GetAllProductsAsync();
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <returns></returns>
        CallResult<LiquidQuoineProduct> GetProduct(int id);
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineProduct>> GetProductAsync(int id);

        /// <summary>
        /// Get orderbook.
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="full">if true, returns full order book, else first 20 levels</param>
        /// <returns></returns>
        CallResult<LiquidQuoineOrderBook> GetOrderBook(int id, bool full);
        /// <summary>
        /// Get orderbook.
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="full">if true, returns full order book, else first 20 levels</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineOrderBook>> GetOrderBookAsync(int id, bool full);

        /// <summary>
        /// Ping to see if the server is reachable
        /// </summary>
        /// <returns>The roundtrip time of the ping request</returns>
        CallResult<long> Ping();

        /// <summary>
        /// Ping to see if the server is reachable
        /// </summary>
        /// <returns>The roundtrip time of the ping request</returns>
        Task<CallResult<long>> PingAsync();

        void Dispose();
    }
}
