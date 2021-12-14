﻿using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects
{

    public class LiquidQuoineBase
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_at"),JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime? UpdatedAt { get; set; }

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
