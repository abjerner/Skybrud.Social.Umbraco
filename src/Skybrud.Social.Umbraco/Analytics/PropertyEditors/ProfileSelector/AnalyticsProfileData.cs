using System;
using Newtonsoft.Json;

namespace Skybrud.Social.Umbraco.Analytics.PropertyEditors.ProfileSelector {

    public class AnalyticsProfileData {

        #region Properties
        
        [JsonProperty("account")]
        public string AccountId { get; set; }

        [JsonProperty("webProperty")]
        public string WebPropertyId { get; set; }

        [JsonProperty("profile")]
        public string ProfileId { get; set; }

        [JsonIgnore]
        public bool HasData {
            get {
                return (
                    !String.IsNullOrWhiteSpace(AccountId)
                    &&
                    !String.IsNullOrWhiteSpace(WebPropertyId)
                    &&
                    !String.IsNullOrWhiteSpace(ProfileId)
                );
            }
        }

        #endregion

        #region Methods

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }

        public static AnalyticsProfileData Deserialize(string str) {
            return JsonConvert.DeserializeObject<AnalyticsProfileData>(str);
        }

        #endregion

    }

}