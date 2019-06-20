using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Converters.Time;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Time;
using Skybrud.Social.Facebook;
using Skybrud.Social.Facebook.Models.Authentication;
using Skybrud.Social.Facebook.Models.Users;
using Skybrud.Social.Umbraco.Controllers.Surface;

namespace Skybrud.Social.Umbraco.Models.Facebook {

    public class FacebookOAuthData {

        #region Properties

        [JsonProperty("authenticatedAt")]
        public EssentialsTime AuthenticatedAt { get; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; }

        [JsonProperty("expiresIn")]
        [JsonConverter(typeof(TimeSpanSecondsConverter))]
        public TimeSpan ExpiresIn { get; }

        [JsonProperty("user")]
        [JsonConverter(typeof(JsonObjectBaseConverter))]
        public FacebookUser User { get; }

        /// <summary>
        /// Gets whether the property value contains information about an authenticated Facebook user.
        /// </summary>
        [JsonIgnore]
        public bool IsAuthenticated => String.IsNullOrWhiteSpace(AccessToken);

        #endregion

        #region Constructors

        internal FacebookOAuthData(FacebookToken token, FacebookUser user) {
            AuthenticatedAt = EssentialsTime.UtcNow;
            AccessToken = token.AccessToken;
            ExpiresIn = token.ExpiresIn;
            User = user;
        }

        public FacebookOAuthData(JObject obj) {
            AuthenticatedAt = obj.GetString("authenticatedAt", EssentialsTime.Parse);
            AccessToken = obj.GetString("accessToken");
            ExpiresIn = obj.GetDouble("expiresIn", TimeSpan.FromSeconds);
            User = obj.GetObject("user", FacebookUser.Parse);
        }

        #endregion

        #region Member methods

        public FacebookService GetService() {
            if (IsAuthenticated) throw new Exception("User is not authenticated");
            return FacebookService.CreateFromAccessToken(AccessToken);
        }

        #endregion

        #region Static methods

        public static FacebookOAuthData Parse(JObject obj) {
            return obj == null ? null : new FacebookOAuthData(obj);

        }

        public static FacebookOAuthData Deserialize(string value) {
            try {
                return Parse(JsonUtils.ParseJsonObject(value));
            } catch {
                return Parse(new JObject());
            }
        }

        #endregion

    }

}