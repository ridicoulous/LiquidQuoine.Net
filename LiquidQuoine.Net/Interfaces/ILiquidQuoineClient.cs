using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using LiquidQuoine.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
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

        CallResult<LiquidQouineAccountCurrencyBalance> GetAccountBalance(string currency);
        Task<CallResult<LiquidQouineAccountCurrencyBalance>> GetAccountBalanceAsync(string currency, CancellationToken ct = default);


        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        CallResult<List<LiquidQuoineProduct>> GetAllProducts();
        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        Task<CallResult<List<LiquidQuoineProduct>>> GetAllProductsAsync(CancellationToken ct = default);
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <returns></returns>
        CallResult<LiquidQuoineProduct> GetProduct(int id);
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineProduct>> GetProductAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">Set true to get all price levels (default is 20 each side)</param>
        /// <returns>LiquidQuoineOrderBook</returns>
        CallResult<LiquidQuoineOrderBook> GetOrderBook(int id, bool full);
        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">Set true to get all price levels (default is 20 each side)</param>
        /// <returns>LiquidQuoineOrderBook</returns>
        Task<CallResult<LiquidQuoineOrderBook>> GetOrderBookAsync(int id, bool full, CancellationToken ct = default);

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
        Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetExecutionsAsync(int id, int? limit=null, int? page=null, CancellationToken ct = default);

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
        Task<CallResult<List<LiquidQuoineExecution>>> GetExecutionsAsync(int productId, DateTime dateFrom, int? limit = null, CancellationToken ct = default);

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
        Task<CallResult<LiquidQuoineInterestRate>> GetInterestRatesAsync(string currency, CancellationToken ct = default);
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
        CallResult<LiquidQuoinePlacedOrder> PlaceOrder(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal? price = null, decimal? priceRange = null);
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
        Task<CallResult<LiquidQuoinePlacedOrder>> PlaceOrderAsync(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal? price = null, decimal? priceRange=null, CancellationToken ct = default);
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
        Task<CallResult<LiquidQuoinePlacedOrder>> PlaceMarginOrderAsync(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel, string fundingCurrency,  decimal quantity, decimal price, decimal? priceRange = null, OrderDirection? orderDirection = null, CancellationToken ct = default);
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
        Task<CallResult<LiquidQuoinePlacedOrder>> GetOrderAsync(long orderId, CancellationToken ct = default);
        /// <summary>
        /// Cancel placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        CallResult<LiquidQuoinePlacedOrder> CancelOrder(long orderId);
        /// <summary>
        /// Cancel placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoinePlacedOrder>> CancelOrderAsync(long orderId, CancellationToken ct = default);
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        CallResult<LiquidQuoinePlacedOrder> EditOrder(long orderId,decimal quantity, decimal price);
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        Task<CallResult<LiquidQuoinePlacedOrder>> EditOrderAsync(long orderId, decimal quantity, decimal price, CancellationToken ct = default);
        /// <summary>
        /// Get an order's trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        CallResult<List<LiquidQuoineOrderTrade>> GetOrderTrades(long orderId);
        /// <summary>
        /// Get an order's trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<CallResult<List<LiquidQuoineOrderTrade>>> GetOrderTradesAsync(long orderId, CancellationToken ct = default);
//looks like it is removed from documentation
        ///// <summary>
        ///// Get an Order’s Executions
        ///// </summary>
        ///// <param name="orderId">Order ID</param>
        ///// <param name="limit">Limit executions per request</param>
        ///// <param name="page">Page number of results</param>
        ///// <returns></returns>
        //CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetOrderExecutions(long orderId, int? limit = null, int? page = null);
        ///// <summary>
        ///// Get an Order’s Executions
        ///// </summary>
        ///// <param name="orderId">Order ID</param>
        ///// <param name="limit">Limit executions per request</param>
        ///// <param name="page">Page number of results</param>
        ///// <returns></returns>
        //Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetOrderExecutionsAsync(long orderId, int? limit=null, int? page = null);
        /// <summary>
        /// Get Your Executions by product id
        /// </summary>
        /// <param name="productId">Product id</param>   
        /// <returns></returns>
        CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetMyExecutions(int productId, int? limit = null, int? page = null);
        /// <summary>
        ///  Get Your Executions by product id
        /// </summary>
        /// <param name="productId">Id of product</param>
        /// <param name="limit">limit of count of executions</param>
        /// <param name="page">page of list of pages of executions</param>
        /// <returns></returns>
        Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetMyExecutionsAsync(int productId, int?limit=null, int?page=null, CancellationToken ct = default);



        CallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>> GetOrders(string fundingCurrency=null, int? productId=null, OrderStatus? status=null, bool withDetails= false, int limit = 1000, int page = 1);

        Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>>> GetOrdersAsync(string fundingCurrency = null, int? productId = null, OrderStatus? status = null, bool withDetails = false, int limit = 1000, int page = 1, CancellationToken ct = default);

       

        void Dispose();
    }
}
