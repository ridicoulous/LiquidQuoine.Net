﻿using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using LiquidQuoine.Net;
using LiquidQuoine.Net.Interfaces;
using LiquidQuoine.Net.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineClient : RestClient, ILiquidQuoineClient
    {
        private static LiquidQuoineClientOptions defaultOptions = new LiquidQuoineClientOptions();
        private static LiquidQuoineClientOptions DefaultOptions => defaultOptions.Copy<LiquidQuoineClientOptions>();
        private const string GetAllProductsEndpoint = "products";
        private const string GetProductEndpoint = "products/{}";
        private const string GetOrderBookEndpoint = "products/{}/price_levels";
        private const string GetExecutionsEndpoint = "executions";




        private const string GetAllAccountsBalancesEndpoint = "accounts/balance";


        #region constructor/destructor
        /// <summary>
        /// Create a new instance of HuobiClient using the default options
        /// </summary>
        public LiquidQuoineClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of the HuobiClient with the provided options
        /// </summary>
        public LiquidQuoineClient(LiquidQuoineClientOptions options) : base(options, options.ApiCredentials == null ? null : new LiquidQuoineAuthenticationProvider(options.ApiCredentials))
        {
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
        public LiquidQuoineClient(LiquidQuoineClientOptions exchangeOptions, LiquidQuoineAuthenticationProvider authenticationProvider) : base(exchangeOptions, authenticationProvider)
        {
        }
        #endregion


        
        #region Basic methods
        protected override IRequest ConstructRequest(Uri uri, string method, Dictionary<string, object> parameters, bool signed)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            var uriString = uri.ToString();
            //if (authProvider != null)
            //    parameters = authProvider.AddAuthenticationToParameters(uriString, method, parameters, signed);
            //string pathEndQuery = String.Empty;
            if ((method == Constants.GetMethod || method == Constants.DeleteMethod || postParametersPosition == PostParameters.InUri) && parameters?.Any() == true)
            {
                uriString += "?" + parameters.CreateParamString(true);
            }

            //if (method == Constants.PostMethod && signed)
            //{
            //    var uriParamNames = new[] { "AccessKeyId", "SignatureMethod", "SignatureVersion", "Timestamp", "Signature" };
            //    var uriParams = parameters.Where(p => uriParamNames.Contains(p.Key)).ToDictionary(k => k.Key, k => k.Value);
            //    uriString += "?" + uriParams.CreateParamString(true);
            //    parameters = parameters.Where(p => !uriParamNames.Contains(p.Key)).ToDictionary(k => k.Key, k => k.Value);
            //}

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
                    WriteParamBody(request, parameters);
                else
                    WriteParamBody(request, "{}");
            }

            return request;
        }

        protected override bool IsErrorResponse(JToken data)
        {
            return data.ToString().Contains("errors")|| data.ToString().Contains("message");
        }
        
        protected override Error ParseErrorResponse(JToken error)
        {
            if (error["errors"] == null)
                return new ServerError(error.ToString());

            return new ServerError($"{(string)error["errors"]}");
        }

        protected Uri GetUrl(string endpoint, string version = null)
        {
            return version == null ? new Uri($"{BaseAddress}/{endpoint}") : new Uri($"{BaseAddress}/v{version}/{endpoint}");
        }
        #endregion
        public  CallResult<List<LiquidQouineAccountBalance>> GetAccountsBalances() => GetAccountsBalancesAsync().Result;
        public async Task<CallResult<List<LiquidQouineAccountBalance>>> GetAccountsBalancesAsync()
        {
            var result = await ExecuteRequest<List<LiquidQouineAccountBalance>>(GetUrl(GetAllAccountsBalancesEndpoint), "GET", null, true).ConfigureAwait(false);
            return new CallResult<List<LiquidQouineAccountBalance>>(result.Data, result.Error);
        }
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
            var result = await ExecuteRequest<List<LiquidQuoineProduct>>(GetUrl(GetAllProductsEndpoint), "GET", null, true).ConfigureAwait(false);
            return new CallResult<List<LiquidQuoineProduct>>(result.Data, result.Error);
        }

        public CallResult<LiquidQuoineProduct> GetProduct(int id) => GetProductAsync(id).Result;

        public async Task<CallResult<LiquidQuoineProduct>> GetProductAsync(int id)
        {
            var result = await ExecuteRequest<LiquidQuoineProduct>(GetUrl(FillPathParameter(GetProductEndpoint,id.ToString())), "GET").ConfigureAwait(false);
            return new CallResult<LiquidQuoineProduct>(result.Data, result.Error);
        }

        public CallResult<LiquidQuoineOrderBook> GetOrderBook(int id, bool full=false) => GetOrderBookAsync(id,full).Result;
         
        public async Task<CallResult<LiquidQuoineOrderBook>> GetOrderBookAsync(int id, bool fullOrderbook=false)
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

        public async Task<CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>> GetExecutionsAsync(int id, int? limit = null, int? page = null)
        {
            var parameters = new Dictionary<string, object>() { { "product_id",id } };
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("page", page);
           
            var result = await ExecuteRequest<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(GetUrl(GetExecutionsEndpoint), "GET", parameters).ConfigureAwait(false);
            return new CallResult<LiquidQuoineDefaultResponse<LiquidQuoineExecution>>(result.Data, result.Error);
        }
    }
}
