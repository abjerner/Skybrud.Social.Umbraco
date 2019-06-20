//using System.Collections.Generic;
//using System.Linq;
//using Newtonsoft.Json;
//using Skybrud.Social.Gmail.Scopes;
//using Skybrud.Social.Google.Analytics.Scopes;
//using Skybrud.Social.Google.Scopes;
//using Skybrud.Social.GoogleCalendar.Scopes;
//using Skybrud.Social.GoogleDrive.Scopes;
//using Skybrud.Social.YouTube.Scopes;

//namespace Skybrud.Social.Umbraco.Models.Google {

//    public class GoogleScopeGroup {

//        [JsonProperty("name")]
//        public string Name { get; set; }

//        [JsonProperty("scopes")]
//        public GoogleScopeItem[] Scopes { get; set; }

//        public GoogleScopeGroup(string name, params GoogleScope[] scope) {
//            Name = name;
//            Scopes = scope.Select(x => new GoogleScopeItem(x)).ToArray();
//        }

//        public static GoogleScopeGroup[] GetAll() {

//            List<GoogleScopeGroup> groups = new List<GoogleScopeGroup>();

//            groups.Add(new GoogleScopeGroup("Basic", GoogleScopes.All));
//            groups.Add(new GoogleScopeGroup("Analytics", AnalyticsScopes.All));
//            groups.Add(new GoogleScopeGroup("Calendar", CalendarScopes.All));
//            groups.Add(new GoogleScopeGroup("Drive", DriveScopes.All));
//            groups.Add(new GoogleScopeGroup("Gmail", GmailScopes.All));
//            groups.Add(new GoogleScopeGroup("YouTube", YouTubeScopes.All));

//            return groups.ToArray();

//        }

//    }

//    public class GoogleScopeItem {

//        [JsonProperty("alias")]
//        public string Alias { get; set; }

//        [JsonProperty("name")]
//        public string Name { get; set; }

//        [JsonProperty("description")]
//        public string Description { get; set; }

//        public GoogleScopeItem(GoogleScope scope) {
//            Alias = scope.Alias;
//            Name = scope.Name;
//            Description = scope.Description;
//        }

//    }

//}