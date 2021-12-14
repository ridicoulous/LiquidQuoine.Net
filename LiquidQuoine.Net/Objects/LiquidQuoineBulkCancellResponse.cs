using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{

    public class LiquidQuoineCancelledInBulkOrder : ICommonOrderId
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("client_order_id")]
        public string ClientOrderId { get; set; }

        public string CommonId => Id.ToString();
    }
    public class LiquidQuoineCancelledInBulkResponse<T> where T : LiquidQuoineCancelledInBulkOrder
    {
        [JsonProperty("models")]
        public List<T> Result { get; set; }

    }

}
