using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace LiquidQuoine.Net.Converters
{

    /// <summary>
    /// Custom JSON converter to handle the different types of date formats being returned by Liquid API for a "Trade" object.
    /// In some cases, Liquid returns an ISO 8601 string representation of the datetime. In other cases, Liquid returns the numeric Unix Epoch value.
    /// </summary>
    public class OrderTradeDateTimeConverter : IsoDateTimeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            try
            {
                var t = long.Parse(reader.Value.ToString());
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(t);
            }
            catch
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((long)Math.Round(((DateTime)value - new DateTime(1970, 1, 1)).TotalMilliseconds));
        }
    }
}
