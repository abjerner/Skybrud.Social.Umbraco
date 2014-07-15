using System;
using Newtonsoft.Json;
using Skybrud.Social.Instagram;

namespace Skybrud.Social.Umbraco.Instagram.PropertyEditors.OAuth {

    public class InstagramOAuthData {

        #region Private fields

        private InstagramService _service;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID of the authenticated user.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets the username of the authenticated user.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets the name of the authenticated user.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the full name of the authenticated user.
        /// </summary>
        [JsonProperty("fullname")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets the URL to the profile picture (avatar) of the authenticated user.
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets whether the OAuth data is valid - that is whether the OAuth data has a valid
        /// access token. Calling this property will not check the validate the access token
        /// against the API.
        /// </summary>
        [JsonIgnore]
        public bool IsValid {
            get { return !String.IsNullOrWhiteSpace(AccessToken); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the InstagramService class.
        /// </summary>
        public InstagramService GetService() {
            return _service ?? (_service = InstagramService.CreateFromAccessToken(AccessToken));
        }
        
        /// <summary>
        /// Serializes the OAuth data into a JSON string.
        /// </summary>
        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Deserializes the specified JSON string into an OAuth data object.
        /// </summary>
        /// <param name="str">The JSON string to be deserialized.</param>
        public static InstagramOAuthData Deserialize(string str) {
            return JsonConvert.DeserializeObject<InstagramOAuthData>(str);
        }

        #endregion

    }

}