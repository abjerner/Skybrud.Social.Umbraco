using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybrud.Social.Umbraco.Google.PropertyEditors.OAuth {

    public class GoogleOAuthPreValueOptions {

        [JsonProperty("clientid")]
        public string ClientId { get; set; }

        [JsonProperty("clientsecret")]
        public string ClientSecret { get; set; }

        [JsonProperty("redirecturi")]
        public string RedirectUri { get; set; }

        [JsonProperty("scope")]
        public string[] Scope { get; set; }

        [JsonIgnore]
        public bool IsValid {
            get {
                if (String.IsNullOrEmpty(ClientId)) return false;
                if (String.IsNullOrEmpty(ClientSecret)) return false;
                if (String.IsNullOrEmpty(RedirectUri)) return false;
                return true;
            }
        }

        public static GoogleOAuthPreValueOptions Get(string contentTypeAlias, string propertyAlias) {

            IDictionary<string, string> prevalues = PreValueHelpers.GetPreValues(contentTypeAlias, propertyAlias);

            string config;
            prevalues.TryGetValue("config", out config);

            try {
                return JsonConvert.DeserializeObject<GoogleOAuthPreValueOptions>(config);
            } catch (Exception) {
                return new GoogleOAuthPreValueOptions();
            }

        }

    }

}
