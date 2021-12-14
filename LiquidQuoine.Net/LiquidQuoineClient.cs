using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using LiquidQuoine.Net.Converters;
using LiquidQuoine.Net.Interfaces;
using LiquidQuoine.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineClient : RestClient, ILiquidQuoineClient, IExchangeClient
    {
        #region Endpoints consts
        private const string GetAllProductsEndpoint = "products";
        private const string GetProductEndpoint = "products/{}";
        private const string GetOrderBookEndpoint = "products/{}/price_levels";
        private const string GetExecutionsEndpoint = "executions";
        private const string GetInterestRatesEndpoint = "ir_ladders/{}";
        private const string PlaceOrderEndpoint = "orders";
        private const string GetOrderEndpoint = "orders/{}";
        private const string GetOrderByClOrdIdEndpoint = "orders/client:{}";
        private const string GetOrdersEndpoint = "orders";
        private const string CancelOrderEndpoint = "orders/{}/cancel";
        private const string CancelOrderByClOrdIdEndpoint = "orders/client:{}/cancel";
        private const string CancelOrdersInBulkEndpoint = "orders/cancel_all";
        private const string EditOrderEndpoint = "orders/{}";
        private const string EditOrderByClOrdIdEndpoint = "/orders/client:{}";
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

        public event Action<ICommonOrderId> OnOrderPlaced;
        public event Action<ICommonOrderId> OnOrderCanceled;

        private Dictionary<string, int> pairNameIdCache = new Dictionary<string, int>();

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
        public LiquidQuoineClient(LiquidQuoineClientOptions options) : base("Liquid", options, options.ApiCredentials == null ? null : new LiquidQuoineAuthenticationProvider(options.ApiCredentials))
        {
            log.Level = LogLevel.Debug;
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

        protected override IRequest ConstructRequest(Uri uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, HttpMethodParameterPosition parametersPosition, ArrayParametersSerialization arraySerialization, int requestId, Dictionary<string, string> additionalHeders)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            var uriString = uri.ToString();
            if ((method == HttpMethod.Get || method == HttpMethod.Delete || parametersPosition == HttpMethodParameterPosition.InUri) && parameters?.Any() == true)
            {
                uriString += "?" + parameters.CreateParamString(true, ArrayParametersSerialization.MultipleValues);
            }
            var request = RequestFactory.Create(method, uriString, requestId);
            //  string requestBodyFormat = RequestBodyFormat.Json ? Constants.JsonContentHeader : ;
            request.Accept = Constants.JsonContentHeader;
            request.Method = method;

            var headers = new Dictionary<string, string>();
            if (authProvider != null)
                headers = authProvider.AddAuthenticationToHeaders(new Uri(uriString).PathAndQuery, method, null, signed, parametersPosition, arraySerialization);

            if (additionalHeders != null)
            {
                foreach (var h in additionalHeders)
                {
                    headers[h.Key] = h.Value;
                }
            }

            foreach (var header in headers)
                request.AddHeader(header.Key, header.Value);
            if ((method == HttpMethod.Post || method == HttpMethod.Put) && parametersPosition != HttpMethodParameterPosition.InUri)
            {
                WriteParamBody(request, parameters, Constants.JsonContentHeader);
            }
            return request;
        }

        protected bool IsErrorResponse(JToken data)
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
        public WebCallResult<List<LiquidQuoineProduct>> GetAllProducts() => GetAllProductsAsync().Result;

        /// <summary>
        /// Get the list of all available products.
        /// </summary>
        /// <returns></returns>
        public async Task<WebCallResult<List<LiquidQuoineProduct>>> GetAllProductsAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<List<LiquidQuoineProduct>>(GetUrl(GetAllProductsEndpoint), HttpMethod.Get, ct, null, false).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a Product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoineProduct> GetProduct(int id) => GetProductAsync(id).Result;
        /// <summary>
        /// Get a Product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoineProduct>> GetProductAsync(int id, CancellationToken ct = default)
        {
            return await SendRequestAsync<LiquidQuoineProduct>(GetUrl(FillPathParameter(GetProductEndpoint, id.ToString())), HttpMethod.Get, ct).ConfigureAwait(false);
        }
        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">if true, get full orderbook</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoineOrderBook> GetOrderBook(int id, bool full = false) => GetOrderBookAsync(id, full).Result;
        /// <summary>
        /// Get Order Book
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="full">if true, get full orderbook</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoineOrderBook>> GetOrderBookAsync(int id, bool fullOrderbook = false, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (fullOrderbook)
                parameters.Add("full", 1);
            return await SendRequestAsync<LiquidQuoineOrderBook>(GetUrl(FillPathParameter(GetOrderBookEndpoint, id.ToString())), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }
        /// <summary>
        /// Get a list of recent executions from a product (Executions are sorted in DESCENDING order - Latest first)
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <param name="page">Page number from all results</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetExecutions(int id, int? limit = null, int? page = null) => GetExecutionsAsync(id, limit, page).Result;
        /// <summary>
        /// Get a list of recent executions from a product (Executions are sorted in DESCENDING order - Latest first)
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <param name="page">Page number from all results</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetExecutionsAsync(int id, int? limit = null, int? page = null, CancellationToken ct = default)
        {
            if (limit > 1000)
            {
                limit = 1000;
                log.Write(LogLevel.Warning, $"Limit should be between 1 and 1000, changed to {limit}");
            }
            else if (limit < 1)
            {
                limit = 20; 
                log.Write(LogLevel.Warning, $"Limit should be between 1 and 1000, changed to {limit}");
            }
            var parameters = new Dictionary<string, object>() { { "product_id", id } };
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("page", page);
            return await SendRequestAsync<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(GetExecutionsEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }
        /// <summary>
        /// Get a list of executions after a particular time (Executions are sorted in ASCENDING order)
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="dateFrom">Only show executions at or after this timestamp (Unix timestamps in seconds)</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <returns></returns>
        public WebCallResult<List<LiquidQuoineExecution>> GetExecutions(int productId, DateTime dateFrom, int? limit = null) => GetExecutionsAsync(productId, dateFrom, limit).Result;
        /// <summary>
        /// Get a list of executions after a particular time (Executions are sorted in ASCENDING order)
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="dateFrom">Only show executions at or after this timestamp (Unix timestamps in seconds)</param>
        /// <param name="limit">How many executions should be returned. Must be <= 1000. Default is 20</param>
        /// <returns></returns>        
        public async Task<WebCallResult<List<LiquidQuoineExecution>>> GetExecutionsAsync(int productId, DateTime startTime, int? limit = null, CancellationToken ct = default)
        {
            if (limit > 1000)
            {
                limit = 1000;
                log.Write(LogLevel.Warning, $"Limit should be between 1 and 1000, changed to {limit}");
            }
            else if (limit < 1)
            {
                limit = 20;
                log.Write(LogLevel.Warning, $"Limit should be between 1 and 1000, changed to {limit}");
            }
            var parameters = new Dictionary<string, object>() {
                { "product_id", productId },
                { "timestamp", JsonConvert.SerializeObject(startTime, new TimestampSecondsConverter()) }
            };
            parameters.AddOptionalParameter("limit", limit);
            return  await SendRequestAsync<List<LiquidQuoineExecution>>(GetUrl(GetExecutionsEndpoint), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }
        /// <summary>
        /// Interest Rates Get Interest Rate Ladder for a currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoineInterestRate> GetInterestRates(string currency) => GetInterestRatesAsync(currency).Result;
        /// <summary>
        /// Interest Rates Get Interest Rate Ladder for a currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoineInterestRate>> GetInterestRatesAsync(string currency, CancellationToken ct = default)
        {
            return await SendRequestAsync<LiquidQuoineInterestRate>(GetUrl(FillPathParameter(GetInterestRatesEndpoint, currency)), HttpMethod.Get, ct).ConfigureAwait(false);
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
        /// <param name="clientOrderId">A self-identified Order ID, 
        ///                             a custom unique identifying JSON string up to 36 bytes with any content (as long as it is unique). 
        ///                             User must avoid special characters besides "-".
        ///                             client_order_id must always be unique and not be reused.</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> PlaceOrder(int productId, OrderSide orderSide, OrderType orderType, decimal quantity, decimal? price = null, decimal? priceRange = null, string clientOrderId = null)
            => PlaceOrderAsync(productId, orderSide, orderType, quantity, price, priceRange, clientOrderId).Result;
        /// <summary>
        /// Create an Order
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="orderSide"></param>
        /// <param name="orderType"></param>
        /// <param name="quantity">quantity to buy or sell</param>
        /// <param name="price">price per unit of cryptocurrency</param>
        /// <param name="priceRange">For order_type of market_with_range only, slippage of the order. Use for TrailingStops</param>
        /// <param name="clientOrderId">A self-identified Order ID, 
        ///                             a custom unique identifying JSON string up to 36 bytes with any content (as long as it is unique). 
        ///                             User must avoid special characters besides "-".
        ///                             client_order_id must always be unique and not be reused.</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> PlaceOrderAsync(int productId,
            OrderSide orderSide,
            OrderType orderType,
            decimal quantity,
            decimal? price = null,
            decimal? priceRange = null,
            string clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "order_type", JsonConvert.SerializeObject(orderType,new OrderTypeConverter())},
                { "product_id", productId },
                { "side",  JsonConvert.SerializeObject(orderSide,new OrderSideConverter())},
                { "quantity",quantity.ToString(CultureInfo.GetCultureInfo("en-US"))}
            };

            if (price.HasValue && orderType == OrderType.Market)
                return WebCallResult<LiquidQuoinePlacedOrder>.CreateErrorResult( new ServerError("price parameter must be used for order type != OrderType.Market"));
            if (priceRange.HasValue && orderType != OrderType.MarketWithRange)
                return WebCallResult<LiquidQuoinePlacedOrder>.CreateErrorResult(new ServerError("priceRange parameter can be used only for OrderType.MarketWithRange only, slippage of the order"));

            parameters.AddOptionalParameter("price", price.HasValue ? price.Value.ToString(CultureInfo.GetCultureInfo("en-US")) : null);
            parameters.AddOptionalParameter("price_range", priceRange);
            parameters.AddOptionalParameter("client_order_id", clientOrderId);
            var order = new Dictionary<string, object>() { { "order", parameters } };
            var test = JsonConvert.SerializeObject(order);
            Console.WriteLine(test);
            var result = await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(PlaceOrderEndpoint), HttpMethod.Post, ct, order, true).ConfigureAwait(false);
            if (result.Success)
            {
                OnOrderPlaced?.Invoke(result.Data);
            }

            return result;
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
        ///  <param name="clientOrderId">A self-identified Order ID, 
        ///                             a custom unique identifying JSON string up to 36 bytes with any content (as long as it is unique). 
        ///                             User must avoid special characters besides "-".
        ///                             client_order_id must always be unique and not be reused.</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> PlaceMarginOrder(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel, string fundingCurrency, decimal quantity, decimal price, decimal? priceRange = null, OrderDirection? orderDirection = null, string clientOrderId = null)
                => PlaceMarginOrderAsync(productId, orderSide, orderType, leverageLevel, fundingCurrency, quantity, price, priceRange, orderDirection, clientOrderId).Result;
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
        /// <param name="clientOrderId">A self-identified Order ID, 
        ///                             a custom unique identifying JSON string up to 36 bytes with any content (as long as it is unique). 
        ///                             User must avoid special characters besides "-".
        ///                             client_order_id must always be unique and not be reused.</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> PlaceMarginOrderAsync(int productId, OrderSide orderSide, OrderType orderType, LeverageLevel leverageLevel,
            string fundingCurrency, decimal quantity, decimal price, decimal? priceRange = null, OrderDirection? orderDirection = null, string clientOrderId = null, CancellationToken ct = default)
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
                return WebCallResult<LiquidQuoinePlacedOrder>.CreateErrorResult(new ServerError("priceRange parameter can be used only for OrderType.MarketWithRange only, slippage of the order"));
            parameters.AddOptionalParameter("price_range", priceRange);
            parameters.AddOptionalParameter("order_direction", orderDirection);
            parameters.AddOptionalParameter("client_order_id", clientOrderId);
            var order = new Dictionary<string, object>() { { "order", parameters } };

            var result = await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(PlaceOrderEndpoint), HttpMethod.Post, ct, order, true).ConfigureAwait(false);
            if (result.Success)
            {
                OnOrderPlaced?.Invoke(result.Data);
            }

            return result;

        }
        public WebCallResult<LiquidQuoinePlacedOrder> GetOrder(long orderId) => GetOrderAsync(orderId).Result;
        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            return await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(GetOrderEndpoint, orderId.ToString())), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="clientOrderId">client order id</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> GetOrderByClientOrderId(string clientOrderId) => GetOrderByClientOrderIdAsync(clientOrderId).Result;
        /// <summary>
        /// Get placed order
        /// </summary>
        /// <param name="clientOrderId">client order id</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> GetOrderByClientOrderIdAsync(string clientOrderId, CancellationToken ct = default)
        {
            return await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(GetOrderByClOrdIdEndpoint, clientOrderId)), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
        }
       
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> CancelOrder(long orderId) => CancelOrderAsync(orderId).Result;
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(CancelOrderEndpoint, orderId.ToString())), HttpMethod.Put, ct, null, true).ConfigureAwait(false);
            if (result.Success)
            {
                OnOrderCanceled?.Invoke(result.Data);
            }
            return result;
        }
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> CancelOrderByClientOrderId(string clientOrderId) => CancelOrderByClientOrderIdAsync(clientOrderId).Result;
        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> CancelOrderByClientOrderIdAsync(string clientOrderId, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(CancelOrderByClOrdIdEndpoint, clientOrderId.ToString())), HttpMethod.Put, ct, null, true).ConfigureAwait(false);
            if (result.Success)
            {
                OnOrderCanceled?.Invoke(result.Data);
            }
            return result;
        }
        /// <summary>
        /// Cancel all open orders in bulk.
        /// Below are optional body parameters, not specifying any body parameters will result in cancelling all open orders
        /// regardless of Product ID, Trading Type, or Side.
        /// This method does not cancel conditional orders (take profit and stop loss on positions)
        /// </summary>
        /// <returns>List of all orders that were cancelled.</returns>
        public WebCallResult<List<LiquidQuoineCancelledInBulkOrder>> CancelAll(int? productId = null, TradingType? type = null, OrderSide? side = null) => CancelAllAsync(productId, type, side).Result;

        /// <summary>
        /// Cancel all open orders in bulk.
        /// Below are optional body parameters, not specifying any body parameters will result in cancelling all open orders
        /// regardless of Product ID, Trading Type, or Side.
        /// This method does not cancel conditional orders (take profit and stop loss on positions)
        /// </summary>
        /// <returns>List of all orders that were cancelled.</returns>
        public async Task<WebCallResult<List<LiquidQuoineCancelledInBulkOrder>>> CancelAllAsync(int? productId = null, TradingType? type = null, OrderSide? side = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("product_id", productId);
            if (type != null)
                parameters.AddOptionalParameter("trading_type", JsonConvert.SerializeObject(type, new TradingTypeConverter()));
            if (side != null)
                parameters.AddOptionalParameter("side", JsonConvert.SerializeObject(side, new OrderSideConverter()));

            var result = await SendRequestAsync<LiquidQuoineCancelledInBulkResponse<LiquidQuoineCancelledInBulkOrder>>(GetUrl(CancelOrdersInBulkEndpoint), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false); ;
            List<LiquidQuoineCancelledInBulkOrder> orderList = null;
            if (result.Success)
            {
                orderList = result.Data.Result;
                foreach (var item in orderList)
                {
                    OnOrderCanceled?.Invoke(item);
                }
            }
            return result.As<List<LiquidQuoineCancelledInBulkOrder>>(orderList);
        }

        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> EditOrder(long orderId, decimal quantity, decimal price) => EditOrderAsync(orderId, quantity, price).Result;
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> EditOrderAsync(long orderId, decimal quantity, decimal price, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("quantity", quantity);
            parameters.Add("price", price);

            var result = await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(EditOrderEndpoint, orderId.ToString())), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
            return result;
        }
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="clientOrderId">client order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        public WebCallResult<LiquidQuoinePlacedOrder> EditOrderByClientOrderId(string clientOrderId, decimal quantity, decimal price) => EditOrderByClientOrderIdAsync(clientOrderId, quantity, price).Result;
        /// <summary>
        /// Edit placed order
        /// </summary>
        /// <param name="clientOrderId">client order id</param>
        /// <param name="quantity">new order quantity</param>
        /// <param name="price">new order price</param>        
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoinePlacedOrder>> EditOrderByClientOrderIdAsync(string clientOrderId, decimal quantity, decimal price, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("quantity", quantity);
            parameters.Add("price", price);

            var result = await SendRequestAsync<LiquidQuoinePlacedOrder>(GetUrl(FillPathParameter(EditOrderByClOrdIdEndpoint, clientOrderId)), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get an order's trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public WebCallResult<List<LiquidQuoineOrderTrade>> GetOrderTrades(long orderId) => GetOrderTradesAsync(orderId).Result;
        /// <summary>
        /// Get an order's trades
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<WebCallResult<List<LiquidQuoineOrderTrade>>> GetOrderTradesAsync(long orderId, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<List<LiquidQuoineOrderTrade>>(GetUrl(FillPathParameter(GetOrderTradesEndpoint, orderId.ToString())), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result;
        }
        ///// <summary>
        ///// Get an Order’s Executions
        ///// </summary>
        ///// <param name="orderId">Order ID</param>
        ///// <param name="limit">Limit executions per request</param>
        ///// <param name="page">Page number of results</param>
        ///// <returns></returns>
        //public WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetOrderExecutions(long orderId, int? limit=null, int? page=null) => GetOrderExecutionsAsync(orderId, limit, page).Result;
        ///// <summary>
        ///// Get an Order’s Executions
        ///// </summary>
        ///// <param name="orderId">Order ID</param>
        ///// <param name="limit">Limit executions per request</param>
        ///// <param name="page">Page number of results</param>
        ///// <returns></returns>
        //public async Task<WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetOrderExecutionsAsync(long orderId, int? limit=null, int? page=null)
        //{
        //    var parameters = new Dictionary<string, object>();
        //    parameters.AddOptionalParameter("limit", limit);
        //    parameters.AddOptionalParameter("page", page);
        //    var result = await SendRequest<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(FillPathParameter(GetOrderExecutionsEndpoint, orderId.ToString())),HttpMethod.Get, parameters, true).ConfigureAwait(false);
        //    return new WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(result.Data, result.Error);
        //}
        /// <summary>
        /// Get Your Executions by product id
        /// </summary>
        /// <param name="productId">Product id</param>   
        /// <returns></returns>
        public WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>> GetMyExecutions(int productId, int? limit = null, int? page = null) => GetMyExecutionsAsync(productId, limit, page).Result;
        /// <summary>
        /// Get Your Executions by product id
        /// </summary>
        /// <param name="productId">Product id</param>   
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetMyExecutionsAsync(int productId, int? limit = null, int? page = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("product_id", productId);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("page", page);
            var result = await SendRequestAsync<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(GetMyExecutionsEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get all Fiat Account Balances
        /// </summary>
        /// <returns></returns>
        public WebCallResult<List<LiquidQuoineFiatAccount>> GetFiatAccountsBalances() => GetFiatAccountsBalancesAsync().Result;
        /// <summary>
        /// Get all Fiat Account Balances
        /// </summary>
        /// <returns></returns>
        public async Task<WebCallResult<List<LiquidQuoineFiatAccount>>> GetFiatAccountsBalancesAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<List<LiquidQuoineFiatAccount>>(GetUrl(GetFiatAccountsEndpoint), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get all Crypto Account Balances
        /// </summary>
        /// <returns></returns>
        public WebCallResult<List<LiquidQuoineCryptoAccount>> GetCryptoAccountsBalances() => GetCryptoAccountsBalancesAsync().Result;
        /// <summary>
        /// Get all Crypto Account Balances
        /// </summary>
        /// <returns></returns>
        public async Task<WebCallResult<List<LiquidQuoineCryptoAccount>>> GetCryptoAccountsBalancesAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<List<LiquidQuoineCryptoAccount>>(GetUrl(GetCryptoAccountsEndpoint), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Get all Account Balances - without reserved info
        /// </summary>
        /// <returns></returns>
        public WebCallResult<List<LiquidQouineAccountBalance>> GetAccountsBalances() => GetAccountsBalancesAsync().Result;
        /// <summary>
        /// Get all Account Balances - without reserved info
        /// </summary>
        /// <returns></returns>
        public async Task<WebCallResult<List<LiquidQouineAccountBalance>>> GetAccountsBalancesAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<List<LiquidQouineAccountBalance>>(GetUrl(GetAllAccountsBalancesEndpoint), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Close a trade
        /// </summary>
        /// <param name="tradeId">Trade ID</param>
        /// <param name="quantity">The quantity you want to close</param>
        /// <returns></returns>
        public WebCallResult<LiquidQuoineOrderTrade> CloseTrade(long tradeId, decimal? quantity = null) => CloseTradeAsync(tradeId, quantity).Result;
        /// <summary>
        /// Close a trade
        /// </summary>
        /// <param name="tradeId">Trade ID</param>
        /// <param name="quantity">The quantity you want to close</param>
        /// <returns></returns>
        public async Task<WebCallResult<LiquidQuoineOrderTrade>> CloseTradeAsync(long tradeId, decimal? quantity = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("closed_quantity", quantity);

            var result = await SendRequestAsync<LiquidQuoineOrderTrade>(GetUrl(FillPathParameter(CloseTradeEndpoint, tradeId.ToString())), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        #endregion
        public WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>> GetOrders(string fundingCurrency = null, int? productId = null, OrderStatus? status = null, bool withDetails = false, int limit = 1000, int page = 1) => GetOrdersAsync(fundingCurrency, productId, status, withDetails, limit, page).Result;

        public async Task<WebCallResult<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>>> GetOrdersAsync(string fundingCurrency = null,
            int? productId = null, OrderStatus? status = null, bool withDetails = false, int limit = 100, int page = 1, CancellationToken ct = default)
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
            var result = await SendRequestAsync<LiquidQuoineDefaultResponse<LiquidQuoinePlacedOrder>>(GetUrl(GetOrdersEndpoint), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        public WebCallResult<LiquidQouineAccountCurrencyBalance> GetAccountBalance(string currency) => GetAccountBalanceAsync(currency).Result;

        public async Task<WebCallResult<LiquidQouineAccountCurrencyBalance>> GetAccountBalanceAsync(string currency, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<LiquidQouineAccountCurrencyBalance>(GetUrl(FillPathParameter(GetAccountBalanceEndpoint, currency)), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            return result;
        }

        #region  IExchangeClient implementation

        public string GetSymbolName(string baseAsset, string quoteAsset)
        {
            return baseAsset + quoteAsset;
        }

        public async Task<WebCallResult<IEnumerable<ICommonSymbol>>> GetSymbolsAsync()
        {
            var result = await GetAllProductsAsync();
            return result.As<IEnumerable<ICommonSymbol>>(result.Data);
        }

        public async Task<WebCallResult<IEnumerable<ICommonTicker>>> GetTickersAsync()
        {
            var result = await GetAllProductsAsync();
            return result.As<IEnumerable<ICommonTicker>>(result.Data);
        }

        public async Task<WebCallResult<ICommonTicker>> GetTickerAsync(string symbol)
        {
            int id;
            try
            {
                id = GetProductIdByName(symbol);
            }
            catch (Exception ex)
            {
                return WebCallResult<ICommonTicker>.CreateErrorResult(new ServerError(ex.ToString()));
            }
            var result = await GetProductAsync(id);
            return result.As<ICommonTicker>(result.Data);
        }

        public Task<WebCallResult<IEnumerable<ICommonKline>>> GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            throw new NotImplementedException();
        }

        public async Task<WebCallResult<ICommonOrderBook>> GetOrderBookAsync(string symbol)
        {
            int id;
            try
            {
                id = GetProductIdByName(symbol);
            }
            catch (Exception ex)
            {
                return WebCallResult<ICommonOrderBook>.CreateErrorResult(new ServerError(ex.ToString()));
            }
            var result = await GetOrderBookAsync(id);
            return result.As<ICommonOrderBook>(result.Data);
        }

        public async Task<WebCallResult<IEnumerable<ICommonRecentTrade>>> GetRecentTradesAsync(string symbol)
        {
            int id;
            try
            {
                id = GetProductIdByName(symbol);
            }
            catch (Exception ex)
            {
                return WebCallResult<IEnumerable<ICommonRecentTrade>>.CreateErrorResult(new ServerError(ex.ToString()));
            }
            var result = await GetExecutionsAsync(id);
            return result.As<IEnumerable<ICommonRecentTrade>>(result ? result.Data.Result : null);
        }

        public async Task<WebCallResult<ICommonOrderId>> PlaceOrderAsync(string symbol, IExchangeClient.OrderSide side, IExchangeClient.OrderType type, decimal quantity, decimal? price = null, string accountId = null)
        {
            int id;
            try
            {
                id = GetProductIdByName(symbol);
            }
            catch (Exception ex)
            {
                return WebCallResult<ICommonOrderId>.CreateErrorResult(new ServerError(ex.ToString()));
            }
            OrderSide liqSide = side switch
            {
                IExchangeClient.OrderSide.Buy => OrderSide.Buy,
                IExchangeClient.OrderSide.Sell => OrderSide.Sell,
                _ => throw new NotImplementedException("Undefined order side")
            };
            OrderType liqType = type switch
            {
                IExchangeClient.OrderType.Limit => OrderType.Limit,
                IExchangeClient.OrderType.Market => OrderType.Market,
                _ => throw new NotImplementedException("Undefined order type. Use market or limit")
            };
            var result = await PlaceOrderAsync(id, liqSide, liqType, quantity, price);
            return result.As<ICommonOrderId>(result.Data);
        }

        public async Task<WebCallResult<ICommonOrder>> GetOrderAsync(string orderId, string symbol = null)
        {
            var result = await GetOrderAsync(ParseToLongOrderId(orderId));
            return result.As<ICommonOrder>(result.Data);
        }

        public async Task<WebCallResult<IEnumerable<ICommonTrade>>> GetTradesAsync(string orderId, string symbol = null)
        {
            var result = await GetOrderTradesAsync(ParseToLongOrderId(orderId));
            return result.As<IEnumerable<ICommonTrade>>(result.Data);
        }

        public async Task<WebCallResult<IEnumerable<ICommonOrder>>> GetOpenOrdersAsync(string symbol = null)
        {
            int? id = null;
            if (symbol != null)
            {
                try
                {
                    id = GetProductIdByName(symbol);
                }
                catch (Exception ex)
                {
                    return WebCallResult<IEnumerable<ICommonOrder>>.CreateErrorResult(new ServerError(ex.ToString()));
                }
            }
            var result = await GetOrdersAsync(productId: id, status: OrderStatus.Live);
            return result.As<IEnumerable<ICommonOrder>>(result ? result.Data.Result : null);
        }

        public async Task<WebCallResult<IEnumerable<ICommonOrder>>> GetClosedOrdersAsync(string symbol = null)
        {
            int? id = null;
            if (symbol != null)
            {
                try
                {
                    id = GetProductIdByName(symbol);
                }
                catch (Exception ex)
                {
                    return WebCallResult<IEnumerable<ICommonOrder>>.CreateErrorResult(new ServerError(ex.ToString()));
                }
            }
            var result = await GetOrdersAsync(productId: id);
            IEnumerable<ICommonOrder> list = null;
            if (result.Success)
            {
                list = result.Data.Result.Where(o => o.Status == OrderStatus.Canceled || o.Status == OrderStatus.Filled);
            }
            return result.As<IEnumerable<ICommonOrder>>(list);
        }

        public async Task<WebCallResult<ICommonOrderId>> CancelOrderAsync(string orderId, string symbol = null)
        {
            var result = await CancelOrderAsync(ParseToLongOrderId(orderId));
            return result.As<ICommonOrderId>(result.Data);
        }

        public async Task<WebCallResult<IEnumerable<ICommonBalance>>> GetBalancesAsync(string accountId = null)
        {
            var result = await GetAccountsBalancesAsync();
            return result.As<IEnumerable<ICommonBalance>>(result.Data);
        }

        #endregion  IExchangeClient implementation

        private int GetProductIdByName(string name)
        {
            string uppercase = name.ToUpper();
            if (!pairNameIdCache.Any())
            {
                var result = GetAllProductsAsync();
                if (result.Result.Success)
                {
                    pairNameIdCache = GetAllProductsAsync().Result?.Data.ToDictionary(p => p.CurrencyPairCode.ToUpper(), p => p.Id);
                }
                else
                {
                    log.Write(LogLevel.Error, "Error while retrieving list of the all products");
                    throw new HttpRequestException("Error while retrieving list of the all products");
                }
            }
            if (!pairNameIdCache.ContainsKey(uppercase))
            {
                log.Write(LogLevel.Error, "Can't find product with CurrencyPairCode equal to " + name);
            }
            return pairNameIdCache[uppercase];
        }
        private long ParseToLongOrderId(string orderId)
        {
            long id;
            if (long.TryParse(orderId, out id))
            {
                return id;
            }
            else
            {
                throw new ArgumentException("Can't convert \"orderId\" to type long");
            }
        }

    }
}
