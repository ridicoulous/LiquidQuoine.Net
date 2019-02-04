using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using LiquidQuoine.Net.Objects;
using System;
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
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">if true, get full orderbook</param>
        /// <returns></returns>
        CallResult<LiquidQuoineOrderBook> GetOrderBook(int id, bool full);
        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">if true, get full orderbook</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineOrderBook>> GetOrderBookAsync(int id, bool full);

        /// <summary>
        /// Get a list of recent executions from a product (Executions are sorted in DESCENDING order - Latest first)
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <param name="page">Page number from all results</param>
        /// <returns></returns>
        CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetExecutions(int id, int? limit = null, int? page = null);
        /// <summary>
        /// Get a list of recent executions from a product (Executions are sorted in DESCENDING order - Latest first)
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <param name="page">Page number from all results</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetExecutionsAsync(int id, int? limit=null, int? page=null);

        /// <summary>
        /// Get a list of executions after a particular time (Executions are sorted in ASCENDING order)
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="dateFrom">Only show executions at or after this timestamp (Unix timestamps in seconds)</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <returns></returns>
        CallResult<List<LiquidQuoineExecution>> GetExecutions(int productId, DateTime dateFrom, int? limit = null);
        /// <summary>
        /// Get a list of executions after a particular time (Executions are sorted in ASCENDING order)
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="dateFrom">Only show executions at or after this timestamp (Unix timestamps in seconds)</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <returns></returns>
        Task<CallResult<List<LiquidQuoineExecution>>> GetExecutionsAsync(int productId, DateTime dateFrom, int? limit = null);

        /// <summary>
        /// Interest Rates Get Interest Rate Ladder for a currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        CallResult<LiquidQuoineInterestRate> GetInterestRates(string currency);
        /// <summary>
        /// Interest Rates Get Interest Rate Ladder for a currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineInterestRate>> GetInterestRatesAsync(string currency);
        /// <summary>
        /// Create an Order
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="orderSide"></param>
        /// <param name="orderType"></param>
        /// <param name="quantity">quantity to buy or sell</param>
        /// <param name="price">price per unit of cryptocurrency</param>
        /// <param name="priceRange">For order_type of market_with_range only, slippage of the order. Use for TrailingStops</param>
        /// <returns></returns>
        CallResult<LiquidQuoinePlacedOrder> PlaceOrder(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal price, decimal? priceRange = null);
        /// <summary>
        /// Create an Order
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="orderSide"></param>
        /// <param name="orderType"></param>
        /// <param name="quantity">quantity to buy or sell</param>
        /// <param name="price">price per unit of cryptocurrency</param>
        /// <param name="priceRange">For order_type of market_with_range only, slippage of the order. Use for TrailingStops</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoinePlacedOrder>> PlaceOrderAsync(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal price, decimal? priceRange=null);
        /// <summary>
        /// Use it to place margin order
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="orderSide"></param>
        /// <param name="orderType"></param>
        /// <param name="leverageLevel">Valid levels: 2,4,5,10,25</param>
        /// <param name="fundingCurrency">Currency used to fund the trade with. Default is quoted currency (e.g a trade in BTCUSD product will use USD as the funding currency as default)</param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="priceRange"></param>
        /// <param name="orderDirection">one_direction, two_direction or netout.</param>
        /// <returns></returns>
        CallResult<LiquidQuoinePlacedOrder> PlaceMarginOrder(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel, string fundingCurrency, decimal quantity, decimal price, decimal? priceRange = null,OrderDirection? orderDirection = null);
        /// <summary>
        /// Use it to place margin order
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="orderSide"></param>
        /// <param name="orderType"></param>
        /// <param name="leverageLevel">Valid levels: 2,4,5,10,25</param>
        /// <param name="fundingCurrency">Currency used to fund the trade with. Default is quoted currency (e.g a trade in BTCUSD product will use USD as the funding currency as default)</param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="priceRange"></param>
        /// <param name="orderDirection">one_direction, two_direction or netout.</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoinePlacedOrder>> PlaceMarginOrderAsync(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel, string fundingCurrency,  decimal quantity, decimal price, decimal? priceRange = null, OrderDirection? orderDirection = null);
        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        CallResult<LiquidQuoinePlacedOrder> GetOrder(long orderId);
        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoinePlacedOrder>> GetOrderAsync(long orderId);

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
