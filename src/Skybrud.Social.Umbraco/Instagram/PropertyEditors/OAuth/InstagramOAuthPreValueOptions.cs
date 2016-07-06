using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Social.Instagram;
using Skybrud.Social.Json.Extensions.JObject;

namespace Skybrud.Social.Umbraco.Instagram.PropertyEditors.OAuth {

    public class InstagramOAuthPreValueOptions {

        [JsonIgnore]
        public JObject JObject { get; private set; }

        [JsonProperty("clientid")]
        public string ClientId { get; private set; }

        [JsonProperty("clientsecret")]
        public string ClientSecret { get; private set; }

        [JsonProperty("redirecturi")]
        public string RedirectUri { get; private set; }

        [JsonIgnore]
        public InstagramScope Scope { get; private set; }

        [JsonProperty("scope")]
        public string ScopeStr { get; private set; }

        [JsonIgnore]
        public bool IsValid {
            get {
                if (String.IsNullOrEmpty(ClientId)) return false;
                if (String.IsNullOrEmpty(ClientSecret)) return false;
                if (String.IsNullOrEmpty(RedirectUri)) return false;
                return true;
            }
        }

        public static InstagramOAuthPreValueOptions Get(string contentTypeAlias, string propertyAlias) {

            IDictionary<string, string> prevalues = PreValueHelpers.GetPreValues(contentTypeAlias, propertyAlias);

            string config;
            prevalues.TryGetValue("config", out config);

            try {

                // Parse the JSON for the config/prevalues
                JObject obj = JObject.Parse(config);

                // Determine the scope
                InstagramScope scope = default(InstagramScope);
                foreach (string alias in (obj.GetString("scope") ?? "").Split(',')) {
                    switch (alias) {
                        case "public_content": scope |= InstagramScope.PublicContent; break;
                        //case "follower_list": scope |= InstagramScope.FollowerList; break;
                        case "comments": scope |= InstagramScope.Comments; break;
                        case "relationships": scope |= InstagramScope.Relationships; break;
                        case "likes": scope |= InstagramScope.Likes; break;
                    }
                }

                // Initialize a new instance of the options class
                return new InstagramOAuthPreValueOptions {
                    JObject = obj,
                    ClientId = obj.GetString("clientid"),
                    ClientSecret = obj.GetString("clientsecret"),
                    RedirectUri = obj.GetString("redirecturi"),
                    Scope = scope,
                    ScopeStr = obj.GetString("scope") ?? ""
                };
            
            } catch (Exception) {
                return new InstagramOAuthPreValueOptions();
            }

        }

    }

}
