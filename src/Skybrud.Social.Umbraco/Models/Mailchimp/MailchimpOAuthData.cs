using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Time;
using Skybrud.Social.Mailchimp;
using Skybrud.Social.Mailchimp.Models.Authentication;
using Skybrud.Social.Umbraco.Controllers.Surface;

namespace Skybrud.Social.Umbraco.Models.Mailchimp {

    public class MailchimpOAuthData {

        #region Properties

        [JsonProperty("authenticatedAt")]
        public EssentialsTime AuthenticatedAt { get; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; }

        [JsonProperty("meta")]
        [JsonConverter(typeof(JsonObjectBaseConverter))]
        public MailchimpMetadata Metadata { get; }

        /// <summary>
        /// Gets whether the property value contains information about an authenticated Mailchimp user.
        /// </summary>
        [JsonIgnore]
        public bool IsAuthenticated => String.IsNullOrWhiteSpace(AccessToken) == false && String.IsNullOrWhiteSpace(Metadata?.ApiEndpoint) == false;

        #endregion

        #region Constructors

        internal MailchimpOAuthData(MailchimpTokenInfo token, MailchimpMetadata meta) {
            AuthenticatedAt = EssentialsTime.UtcNow;
            AccessToken = token.AccessToken;
            Metadata = meta;
        }

        public MailchimpOAuthData(JObject obj) {
            AuthenticatedAt = obj.GetString("authenticatedAt", EssentialsTime.Parse);
            AccessToken = obj.GetString("accessToken");
            Metadata = obj.GetObject("meta", MailchimpMetadata.Parse);
        }

        #endregion

        #region Member methods

        public MailchimpService GetService() {
            if (IsAuthenticated) throw new Exception("User is not authenticated");
            return MailchimpService.GetFromAccessToken(AccessToken, Metadata.ApiEndpoint);
        }

        #endregion

        #region Static methods

        public static MailchimpOAuthData Parse(JObject obj) {
            return obj == null ? null : new MailchimpOAuthData(obj);

        }

        public static MailchimpOAuthData Deserialize(string value) {
            try {
                return Parse(JsonUtils.ParseJsonObject(value));
            } catch {
                return Parse(new JObject());
            }
        }

        #endregion

    }

}