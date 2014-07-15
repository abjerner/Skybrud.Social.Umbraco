using System;
using Newtonsoft.Json;
using Skybrud.Social.Twitter;
using Skybrud.Social.Twitter.OAuth;

namespace Skybrud.Social.Umbraco.Twitter.PropertyEditors.OAuth {

    public class TwitterOAuthData {

        #region Private fields

        private TwitterService _service;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID of the authenticated user.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets the screen name of the authenticated user.
        /// </summary>
        [JsonProperty("screenName")]
        public string ScreenName { get; set; }

        /// <summary>
        /// Gets the name of the authenticated user.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the URL to the profile picture (avatar) of the authenticated user.
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// Gets the consumer key of the app used to authenticate the user.
        /// </summary>
        [JsonProperty("consumerKey")]
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Gets the consumer secret of the app used to authenticate the user.
        /// </summary>
        [JsonProperty("consumerSecret")]
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Gets the access token of the authenticated user.
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets the access token secret of the authenticated user.
        /// </summary>
        [JsonProperty("accessTokenSecret")]
        public string AccessTokenSecret { get; set; }

        /// <summary>
        /// Gets whether the OAuth data is valid - meaning that it has a consumer ID, consumer
        /// secret, access token and access token secret.
        /// </summary>
        [JsonIgnore]
        public bool IsValid {
            get {
                return (
                    !String.IsNullOrWhiteSpace(ConsumerKey)
                    &&
                    !String.IsNullOrWhiteSpace(ConsumerSecret)
                    &&
                    !String.IsNullOrWhiteSpace(AccessToken)
                    &&
                    !String.IsNullOrWhiteSpace(AccessTokenSecret)
                );
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the TwitterService class. Invoking this method will not
        /// result in any calls to the Twitter API.
        /// </summary>
        public TwitterService GetService() {
            return _service ?? (_service = TwitterService.CreateFromOAuthClient(new TwitterOAuthClient {
                ConsumerKey = ConsumerKey,
                ConsumerSecret = ConsumerSecret,
                Token = AccessToken,
                TokenSecret = AccessTokenSecret
            }));
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
        public static TwitterOAuthData Deserialize(string str) {
            return JsonConvert.DeserializeObject<TwitterOAuthData>(str);
        }

        #endregion

    }

}