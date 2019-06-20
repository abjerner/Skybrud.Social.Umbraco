using System;
using System.Linq;
using Newtonsoft.Json;
//using Skybrud.Social.Google.Scopes;

namespace Skybrud.Social.Umbraco.Json.Converters {

    public class SocialJsonConverter : JsonConverter {

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

            switch (value) {

                //case GoogleScope[] array:
                //    writer.WriteValue(String.Join(" ", from scope in array select scope.Alias));
                //    return;

                default:
                    writer.WriteNull();
                    return;

            }

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) {
            return true;
        }

    }

}