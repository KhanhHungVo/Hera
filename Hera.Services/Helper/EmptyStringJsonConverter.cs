using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hera.Services.Helper
{
    public class EmptyStringJsonConverter : JsonConverter
    {
        private JsonSerializer _stringSerializer = new JsonSerializer();
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(string) == objectType;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = _stringSerializer.Deserialize<string>(reader);
            return string.IsNullOrEmpty(value) ? null : value.Trim();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            _stringSerializer.Serialize(writer, value);
        }
    }
}
