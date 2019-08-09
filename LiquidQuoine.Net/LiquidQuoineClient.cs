using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using LiquidQuoine.Net.Converters;
using LiquidQuoine.Net.Interfaces;
using LiquidQuoine.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineClient : RestClient, ILiquidQuoineClient
    {
        #region Endpoints consts
        private const string GetAllProductsEndpoint = "products";
        private const string GetProductEndpoint = "products/{}";
        private const string GetOrderBookEndpoint = "products/{}/price_levels";
        private const string GetExecutionsEndpoint = "executions";
        private const string GetInterestRatesEndpoint = "ir_ladders/{}";
        private const string PlaceOrderEndpoint = "orders";
        private const string GetOrderEndpoint = "orders/{}";
        private const string GetOrdersEndpoint = "orders";
        private const string CancelOrderEndpoint = "orders/{}/cancel";
        private const string EditOrderEndpoint = "orders/{}";
        private const string GetOrderTradesEndpoint = "orders/{}/trades";
        private const string GetOrderExecutionsEndpoint = "orders/{}/executions";
        private const string GetMyExecutionsEndpoint = "executions/me";
        private const string GetFiatAccountsEndpoint = "fiat_accounts";
        private const string CreateFiatAccountEndpoint = "fiat_accounts";
        private const string GetCryptoAccountsEndpoint = "crypto_accounts";
        private const string GetAllAccountsBalancesEndpoint = "accounts/balance";
        private const string GetAccountBalanceEndpoint = "accounts/{}";

        private const string GetMainAssetEndpoint = "accounts/main_asset";
        private const string CreateLoanBidEndpoint = "loan_bids";
        private const string GetLoanBidsEndpoint = "loan_bids/{}";
        private const string CloseLoanBidEndpoint = "loan_bids/{}/close";
        private const string GetLoansEndpoint = "loans";
        private const string UpdateLoanEndpoint = "loans/{}";
        private const string GetTradingAccountsEndpoint = "trading_accounts";
        private const string GetTradingAccountEndpoint = "trading_accounts/{}";
        private const string UpdateAccountLeveregaLevelEndpoint = "trading_accounts/{}";
        private const string GetTradesEndpoint = "trades";
        private const string CloseTradeEndpoint = "trades/{}/close";
        private const string CloseAllTradesEndpoint = "trades/close_all";
        private const string UpdateTradeEndpoint = "trades/{}";
        private const string GetTradeLoansEndpoint = "trades/{}/loans";
        #endregion
        #region constructor/destructor
        private static LiquidQuoineClientOptions defaultOptions = new LiquidQuoineClientOptions();
        private static LiquidQuoineClientOptions DefaultOptions => defaultOptions.Copy<LiquidQuoineClientOptions>();
        /// <summary>
        /// Create a new instance of LiquidQuoineClient using the default options
        /// </summary>
        public LiquidQuoineClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of the LiquidQuoineClient with the provided options
        /// </summary>
        public LiquidQuoineClient(LiquidQuoineClientOptions options) : base(options, options.ApiCredentials == null ? null : new LiquidQuoineAuthenticationProvider(options.ApiCredentials))
        {
            log.Level = CryptoExchange.Net.Logging.LogVerbosity.Debug;
        }
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(LiquidQuoineClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetAuthenticationProvider(new LiquidQuoineAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
        }


        #endregion



        #region Basic methods
        protected override IRequest ConstructRequest(Uri uri, string method, Dictionary<string, object> parameters, bool signed)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            var uriString = uri.ToString();
            if ((method == Constants.GetMethod || method == Constants.DeleteMethod || postParametersPosition == PostParameters.InUri) && parameters?.Any() == true)
            {
                uriString += "?" + parameters.CreateParamString(true);
            }
            var request = RequestFactory.Create(uriString);
            request.ContentType = requestBodyFormat == RequestBodyFormat.Json ? Constants.JsonContentHeader : Constants.FormContentHeader;
            request.Accept = Constants.JsonContentHeader;
            request.Method = method;

            var headers = new Dictionary<string, string>();
            if (authProvider != null)
                headers = authProvider.AddAuthenticationToHeaders(new Uri(uriString).PathAndQuery, method, null, signed);

            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);
            if ((method == Constants.PostMethod || method == Constants.PutMethod) && postParametersPosition != PostParameters.InUri)
            {
                if (parameters?.Any() == true)
                    WriteParamBody(request, JsonConvert.SerializeObject(parameters));
                else
                    WriteParamBody(request, "{}");
            }
            return request;
        }

        protected override bool IsErrorResponse(JToken data)
        {
            return data.ToString().Contains("errors") || data.ToString().Contains("message");
        }

        protected override Error ParseErrorResponse(JToken error)
        {
            if (error["errors"] == null)
                return new ServerError(error.ToString());

            return new ServerError($"{error["errors"]}");
        }

        protected Uri GetUrl(string endpoint, string version = null)
        {
            return version == null ? new Uri($"{BaseAddress}/{endpoint}") : new Uri($"{BaseAddress}/v{version}/{endpoint}");
        }
        #endregion

        #region implementation

        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        public CallResult<List<LiquidQuoineProduct>> GetAllProducts() => GetAllProductsAsync().Result;

        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<List<LiquidQuoineProduct>>> GetAllProductsAsync()
        {
            var result = await ExecuteRequest<List<LiquidQuoineProduct>>(GetUrl(GetAllProductsEndpoint), "GET", null, false).ConfigureAwait(false);
            return new CallResult<List<LiquidQuoineProduct>>(result.Data, result.Error);
        }
        /// <summary>
        /// Get a Product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        public CallResult<LiquidQuoineProduct> GetProduct(int id) => GetProductAsync(id).Result;
        /// <summary>
        /// Get a Product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoineProduct>> GetProductAsync(int id)
        {
            var result = await ExecuteRequest<LiquidQuoineProduct>(GetUrl(FillPathParameter(GetProductEndpoint, id.ToString())), "GET").ConfigureAwait(false);
            return new CallResult<LiquidQuoineProduct>(result.Data, result.Error);
        }
        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">if true, get full orderbook</param>
        /// <returns></returns>
        public CallResult<LiquidQuoineOrderBook> GetOrderBook(int id, bool full = false) => GetOrderBookAsync(id, full).Result;
        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">if true, get full orderbook</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoineOrderBook>> GetOrderBookAsync(int id, bool fullOrderbook = false)
        {
            var parameters = new Dictionary<string, object>();
            if (fullOrderbook)
                parameters.Add("full", 1);
            var result = await ExecuteRequest<LiquidQuoineOrderBook>(GetUrl(FillPathParameter(GetOrderBookEndpoint, id.ToString())), "GET", parameters).ConfigureAwait(false);
            return new CallResult<LiquidQuoineOrderBook>(result.Data, result.Error);
        }
        /// <summary>
        /// Get a list of recent executions from a product (Executions are sorted in DESCENDING order - Latest first)
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <param name="page">Page number from all results</param>
        /// <returns></returns>
        public CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetExecutions(int id, int? limit = null, int? page = null) => GetExecutionsAsync(id, limit, page).Result;
        /// <summary>
        /// Get a list of recent executions from a product (Executions are sorted in DESCENDING order - Latest first)
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <param name="page">Page number from all results</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetExecutionsAsync(int id, int? limit = null, int? page = null)
        {
            var parameters = new Dictionary<string, object>() { { "product_id", id } };
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("page", page);
            if (limit > 1000 || limit < 1)
                return new CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(null, new ServerError("Limit should be between 1 and 1000"));

            var result = await ExecuteRequest<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(GetExecutionsEndpoint), "GET", parameters).ConfigureAwait(false);
            return new CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(result.Data, result.Error);
        }
        /// <summary>
        /// Get a list of executions after a particular time (Executions are sorted in ASCENDING order)
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="dateFrom">Only show executions at or after this timestamp (Unix timestamps in seconds)</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <returns></returns>
        public CallResult<List<LiquidQuoineExecution>> GetExecutions(int productId, DateTime dateFrom, int? limit = null) => GetExecutionsAsync(productId, dateFrom, limit).Result;
        /// <summary>
        /// Get a list of executions after a particular time (Executions are sorted in ASCENDING order)
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="dateFrom">Only show executions at or after this timestamp (Unix timestamps in seconds)</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <returns></returns>        
        public async Task<CallResult<List<LiquidQuoineExecution>>> GetExecutionsAsync(int productId, DateTime startTime, int? limit = null)
        {
            var parameters = new Dictionary<string, object>() {
                { "product_id", productId },
                { "timestamp", JsonConvert.SerializeObject(startTime, new TimestampSecondsConverter()) }
            };
            parameters.AddOptionalParameter("limit", limit);
            if (limit > 1000 || limit < 1)
                return new CallResult<List<LiquidQuoineExecution>>(null, new ServerError("Limit should be between 1 and 1000"));
            var result = await ExecuteRequest<List<LiquidQuoineExecution>>(GetUrl(GetExecutionsEndpoint), "GET", parameters).ConfigureAwait(false);
            return new CallResult<List<LiquidQuoineExecution>>(result.Data, result.Error);
        }
        /// <summary>
        /// Interest Rates Get Interest Rate Ladder for a currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        public CallResult<LiquidQuoineInterestRate> GetInterestRates(string currency) => GetInterestRatesAsync(currency).Result;
        /// <summary>
        /// Interest Rates Get Interest Rate Ladder for a currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoineInterestRate>> GetInterestRatesAsync(string currency)
        {
            var result = await ExecuteRequest<LiquidQuoineInterestRate>(GetUrl(FillPathParameter(GetInterestRatesEndpoint, currency)), "GET").ConfigureAwait(false);
            return new CallResult<LiquidQuoineInterestRate>(result.Data, result.Error);
        }
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
        public CallResult<LiquidQuoinePlacedOrder> PlaceOrder(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal? price = null, decimal? priceRange = null)
            => PlaceOrderAsync(productId, orderSide, orderType, quantity, price, priceRange).Result;
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
        public async Task<CallResult<LiquidQuoinePlacedOrder>> PlaceOrderAsync(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal? price = null, decimal? priceRange = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "order_type", JsonConvert.SerializeObject(orderType,new OrderTypeConverter())},
                { "product_id", productId},
                { "side",  JsonConvert.SerializeObject(orderSide,new OrderSideConverter())},
                { "quantity",quantity}
            };

            if (price.HasValue && orderType == OrderType.Market)
                return new CallResult<LiquidQuoinePlacedOrder>(null, new ServerError("price parameter must be used for order type != OrderType.Market"));
            if (priceRange.HasValue && orderType != OrderType.MarketWithRange)
                return new CallResult<LiquidQuoinePlacedOrder>(null, new ServerError("priceRange parameter can be used only for OrderType.MarketWithRange only, slippage of the order"));

            parameters.AddOptionalParameter("price", price);
            parameters.AddOptionalParameter("price_range", priceRange);
            var order = new Dictionary<string, object>() { { "order", parameters } };
            var test = JsonConvert.SerializeObject(order);
            var result = await ExecuteRequest<LiquidQuoinePlacedOrder>(GetUrl(PlaceOrderEndpoint), "POST", order, true, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoinePlacedOrder>(result.Data, result.Error);
        }
        /// <summary>
        /// Use it to place margin order
        /// </summary>
        /// <param name="productId">product id</param>
        /// <param name="orderSide">order side</param>
        /// <param name="orderType">order type</param>
        /// <param name="leverageLevel">Valid levels: 2,4,5,10,25</param>
        /// <param name="fundingCurrency">Currency used to fund the trade with. Default is quoted currency (e.g a trade in BTCUSD product will use USD as the funding currency as default)</param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="priceRange">use it to place TrailingStop order</param>
        /// <param name="orderDirection">one_direction, two_direction or netout.</param>
        /// <returns></returns>
        public CallResult<LiquidQuoinePlacedOrder> PlaceMarginOrder(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel, string fundingCurrency, decimal quantity, decimal price, decimal? priceRange = null, OrderDirection? orderDirection = null)
                => PlaceMarginOrderAsync(productId, orderSide, orderType, leverageLevel, fundingCurrency, quantity, price, priceRange, orderDirection).Result;
        /// <summary>
        /// Use it to place margin order
        /// </summary>
        /// <param name="productId">product id</param>
        /// <param name="orderSide">order side</param>
        /// <param name="orderType">order type</param>
        /// <param name="leverageLevel">Valid levels: 2,4,5,10,25</param>
        /// <param name="fundingCurrency">Currency used to fund the trade with. Default is quoted currency (e.g a trade in BTCUSD product will use USD as the funding currency as default)</param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="priceRange">use it to place TrailingStop order</param>
        /// <param name="orderDirection">one_direction, two_direction or netout.</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoinePlacedOrder>> PlaceMarginOrderAsync(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel, string fundingCurrency, decimal quantity, decimal price, decimal? priceRange = null, OrderDirection? orderDirection = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "order_type", JsonConvert.SerializeObject(orderType,new OrderTypeConverter())},
                { "product_id", productId},
                { "side", JsonConvert.SerializeObject(orderSide,new OrderSideConverter())},
                { "quantity",quantity},
                { "price", price},
                { "leverage_level", JsonConvert.SerializeObject(leverageLevel)},
                { "funding_currency", fundingCurrency},
            };
            if (priceRange.HasValue && orderType != OrderType.MarketWithRange)
                return new CallResult<LiquidQuoinePlacedOrder>(null, new ServerError("priceRange parameter can be used only for OrderType.MarketWithRange only, slippage of the order"));
            parameters.AddOptionalParameter("price_range", priceRange);
            parameters.AddOptionalParameter("order_direction", orderDirection);
            var order = new Dictionary<string, object>() { { "order", parameters } };

            var result = await ExecuteRequest<LiquidQuoinePlacedOrder>(GetUrl(PlaceOrderEndpoint), "POST", order, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoinePlacedOrder>(result.Data, result.Error);

        }
        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        public CallResult<LiquidQuoinePlacedOrder> GetOrder(long orderId) => GetOrderAsync(orderId).Result;
        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoinePlacedOrder>> GetOrderAsync(long orderId)
        {
            var result = await ExecuteRequest<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(GetOrderEndpoint, orderId.ToString())), "GET", null, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoinePlacedOrder>(result.Data, result.Error);
        }
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public CallResult<LiquidQuoinePlacedOrder> CancelOrder(long orderId) => CancelOrderAsync(orderId).Result;
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoinePlacedOrder>> CancelOrderAsync(long orderId)
        {
            var result = await ExecuteRequest<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(CancelOrderEndpoint, orderId.ToString())), "PUT", null, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoinePlacedOrder>(result.Data, result.Error);
        }
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        public CallResult<LiquidQuoinePlacedOrder> EditOrder(long orderId, decimal quantity, decimal price) => EditOrderAsync(orderId, quantity, price).Result;
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoinePlacedOrder>> EditOrderAsync(long orderId, decimal quantity, decimal price)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("quantity", quantity);
            parameters.Add("price", price);

            var result = await ExecuteRequest<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(EditOrderEndpoint, orderId.ToString())), "PUT", parameters, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoinePlacedOrder>(result.Data, result.Error);
        }
        /// <summary>
        /// Get an order's trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public CallResult<List<LiquidQuoineOrderTrade>> GetOrderTrades(long orderId) => GetOrderTradesAsync(orderId).Result;
        /// <summary>
        /// Get an order's trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<CallResult<List<LiquidQuoineOrderTrade>>> GetOrderTradesAsync(long orderId)
        {
            var result = await ExecuteRequest<List<LiquidQuoineOrderTrade>>(GetUrl(FillPathParameter(GetOrderTradesEndpoint, orderId.ToString())), "GET", null, true).ConfigureAwait(false);
            return new CallResult<List<LiquidQuoineOrderTrade>>(result.Data, result.Error);
        }
        ///// <summary>
        ///// Get an Order’s Executions
        ///// </summary>
        ///// <param name="orderId">Order ID</param>
        ///// <param name="limit">Limit executions per request</param>
        ///// <param name="page">Page number of results</param>
        ///// <returns></returns>
        //public CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetOrderExecutions(long orderId, int? limit=null, int? page=null) => GetOrderExecutionsAsync(orderId, limit, page).Result;
        ///// <summary>
        ///// Get an Order’s Executions
        ///// </summary>
        ///// <param name="orderId">Order ID</param>
        ///// <param name="limit">Limit executions per request</param>
        ///// <param name="page">Page number of results</param>
        ///// <returns></returns>
        //public async Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetOrderExecutionsAsync(long orderId, int? limit=null, int? page=null)
        //{
        //    var parameters = new Dictionary<string, object>();
        //    parameters.AddOptionalParameter("limit", limit);
        //    parameters.AddOptionalParameter("page", page);
        //    var result = await ExecuteRequest<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(FillPathParameter(GetOrderExecutionsEndpoint, orderId.ToString())), "GET", parameters, true).ConfigureAwait(false);
        //    return new CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(result.Data, result.Error);
        //}
        /// <summary>
        /// Get Your Executions by product id
        /// </summary>
        /// <param name="productId">Product id</param>   
        /// <returns></returns>
        public CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetMyExecutions(int productId, int? limit = null, int? page = null) => GetMyExecutionsAsync(productId,limit,page).Result;
        /// <summary>
        /// Get Your Executions by product id
        /// </summary>
        /// <param name="productId">Product id</param>   
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetMyExecutionsAsync(int productId, int? limit = null, int? page = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("product_id", productId);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("page", page);
            var result = await ExecuteRequest<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(GetMyExecutionsEndpoint), "GET", parameters, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(result.Data, result.Error);
        }

        /// <summary>
        /// Get all Account Balances
        /// </summary>
        /// <returns></returns>
        public CallResult<List<LiquidQouineAccountBalance>> GetAccountsBalances() => GetAccountsBalancesAsync().Result;
        /// <summary>
        /// Get all Account Balances
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<List<LiquidQouineAccountBalance>>> GetAccountsBalancesAsync()
        {
            var result = await ExecuteRequest<List<LiquidQouineAccountBalance>>(GetUrl(GetAllAccountsBalancesEndpoint), "GET", null, true).ConfigureAwait(false);
            return new CallResult<List<LiquidQouineAccountBalance>>(result.Data, result.Error);
        }

        /// <summary>
        /// Close a trade
        /// </summary>
        /// <param name="tradeId">Trade ID</param>
        /// <param name="quantity">The quantity you want to close</param>
        /// <returns></returns>
        public CallResult<LiquidQuoineOrderTrade> CloseTrade(long tradeId, decimal? quantity = null) => CloseTradeAsync(tradeId, quantity).Result;
        /// <summary>
        /// Close a trade
        /// </summary>
        /// <param name="tradeId">Trade ID</param>
        /// <param name="quantity">The quantity you want to close</param>
        /// <returns></returns>
        public async Task<CallResult<LiquidQuoineOrderTrade>> CloseTradeAsync(long tradeId, decimal? quantity = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("closed_quantity", quantity);

            var result = await ExecuteRequest<LiquidQuoineOrderTrade>(GetUrl(FillPathParameter(CloseTradeEndpoint, tradeId.ToString())), "PUT", parameters, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoineOrderTrade>(result.Data, result.Error);
        }

        #endregion
        public CallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>> GetOrders(string fundingCurrency = null, int? productId = null, OrderStatus? status = null, bool withDetails = false, int limit=1000, int page=1) => GetOrdersAsync(fundingCurrency, productId, status, withDetails, limit, page).Result;

        public async Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>>> GetOrdersAsync(string fundingCurrency = null, int? productId = null, OrderStatus? status = null, bool withDetails = false, int limit=1000, int page=1)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("funding_currency", fundingCurrency);
            parameters.AddOptionalParameter("product_id", productId);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("page", page);


            if (status != null)
            {
                parameters.AddOptionalParameter("status", JsonConvert.SerializeObject(status, new OrderStatusConverter()));
            }
            if (withDetails)
                parameters.AddParameter("with_details", 1);
            var result = await ExecuteRequest<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>>(GetUrl(GetOrdersEndpoint), "GET", parameters, true).ConfigureAwait(false);
            return new CallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>>(result.Data, result.Error);
        }

        public CallResult<List<LiquidQouineAccountCurrencyBalance>> GetAccountBalance(string currency) => GetAccountBalanceAsync(currency).Result;

        public async Task<CallResult<List<LiquidQouineAccountCurrencyBalance>>> GetAccountBalanceAsync(string currency)
        {
            var result = await ExecuteRequest<List<LiquidQouineAccountCurrencyBalance>>(GetUrl(FillPathParameter(GetAccountBalanceEndpoint,currency)), "GET", null, true).ConfigureAwait(false);
            return new CallResult<List<LiquidQouineAccountCurrencyBalance>>(result.Data, result.Error);
        }
    }
}
