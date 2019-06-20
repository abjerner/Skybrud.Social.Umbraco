using System;
using Newtonsoft.Json;
using Skybrud.Essentials.Json;

namespace Skybrud.Social.Umbraco.Controllers.Surface {

    public class JsonObjectBaseConverter : JsonConverter {

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

            JsonObjectBase obj = value as JsonObjectBase;

            if (obj?.JObject == null) {
                writer.WriteNull();
                return;
            }

            obj.JObject.WriteTo(writer);

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) {
            return true;
        }

    }

}