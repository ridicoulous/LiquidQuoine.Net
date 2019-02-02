using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{

    public class LiquidQuoineBase
    {
        [JsonProperty("id")]
        public long Id { get; set; }    

      
    }
    public class LiquidQuoineDefaultResponse<T> where T : LiquidQuoineBase
    {
        [JsonProperty("models")]
        public List<T> Result { get; set; }

        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }
    }

}
