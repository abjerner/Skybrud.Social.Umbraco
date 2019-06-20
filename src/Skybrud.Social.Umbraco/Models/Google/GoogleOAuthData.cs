//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Skybrud.Essentials.Json;
//using Skybrud.Essentials.Json.Extensions;
//using Skybrud.Essentials.Time;
//using Skybrud.Social.Google;
//using Skybrud.Social.Google.Analytics.Scopes;
//using Skybrud.Social.Google.Models;
//using Skybrud.Social.Google.Models.Authentication;
//using Skybrud.Social.Google.Scopes;
//using Skybrud.Social.Umbraco.Controllers.Surface;
//using Skybrud.Social.Umbraco.Json.Converters;

//namespace Skybrud.Social.Umbraco.Models.Google {

//    public class GoogleOAuthData {

//        #region Properties

//        [JsonProperty("authenticatedAt")]
//        public EssentialsTime AuthenticatedAt { get; }

//        /// <summary>
//        /// Gets the client ID of the app used to authenticate the user.
//        /// </summary>
//        [JsonProperty("clinetId")]
//        public string ClientId { get; set; }

//        /// <summary>
//        /// Gets the client secret of the app used to authenticate the user.
//        /// </summary>
//        [JsonProperty("clientSecret")]
//        public string ClientSecret { get; set; }

//        [JsonProperty("accessToken")]
//        public string AccessToken { get; }

//        [JsonProperty("refreshToken")]
//        public string RefreshToken { get; }

//        [JsonProperty("scope")]
//        [JsonConverter(typeof(SocialJsonConverter))]
//        public GoogleScope[] Scope { get; }

//        [JsonProperty("user")]
//        [JsonConverter(typeof(JsonObjectBaseConverter))]
//        public GoogleUserInfo User { get; }

//        /// <summary>
//        /// Gets whether the property value contains information about an authenticated Google user.
//        /// </summary>
//        [JsonIgnore]
//        public bool IsAuthenticated => String.IsNullOrWhiteSpace(RefreshToken) != false && User != null;

//        #endregion

//        #region Constructors

//        internal GoogleOAuthData(string clientId, string clientSecret, GoogleToken token, GoogleUserInfo user) {
//            AuthenticatedAt = EssentialsTime.UtcNow;
//            ClientId = clientId;
//            ClientSecret = clientSecret;
//            AccessToken = token.AccessToken;
//            RefreshToken = token.RefreshToken;
//            Scope = GoogleUtils.ParseScopes(token.JObject.GetString("scope"));
//            User = user;
//        }

//        private GoogleOAuthData(JObject obj) {
//            AuthenticatedAt = obj.GetString("authenticatedAt", EssentialsTime.Parse);
//            ClientId = obj.GetString("clientId");
//            ClientSecret = obj.GetString("clientSecret");
//            AccessToken = obj.GetString("accessToken");
//            RefreshToken = obj.GetString("refreshToken");
//            Scope = GoogleUtils.ParseScopes(obj.GetString("scope"));
//            User = obj.GetObject("user", GoogleUserInfo.Parse);
//        }

//        #endregion

//        #region Member methods

//        public GoogleService GetService() {
//            if (IsAuthenticated) throw new Exception("User is not authenticated");
//            return GoogleService.CreateFromRefreshToken(ClientId, ClientSecret, RefreshToken);
//        }

//        #endregion

//        #region Static methods

//        public static GoogleOAuthData Parse(JObject obj) {
//            return obj == null ? null : new GoogleOAuthData(obj);

//        }

//        public static GoogleOAuthData Deserialize(string value) {
//            try {
//                return Parse(JsonUtils.ParseJsonObject(value));
//            } catch {
//                return Parse(new JObject());
//            }
//        }

//        #endregion

//    }

//}