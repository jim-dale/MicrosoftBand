
namespace HealthCloudClient
{
    using System;
    using System.Xml;
    using Newtonsoft.Json;

    public class Iso8601DurationConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = serializer.Deserialize<String>(reader);
            return XmlConvert.ToTimeSpan(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var str = XmlConvert.ToString((TimeSpan)value);
            serializer.Serialize(writer, str);
        }
    }
}
