using Newtonsoft.Json;
using Skybrud.Social.Umbraco.Google.Scope;
using Skybrud.Social.Umbraco.WebApi;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Social.Umbraco.Controllers.Api
{
    [PluginController("SkybrudSocial")]
    [JsonOnlyConfiguration]
    public class GoogleController : UmbracoApiController
    {
        public object GetScopes()
        {
            return new[]
            {
                GoogleScopeGroups.Basic,
                GoogleScopeGroups.Analytics,
                GoogleScopeGroups.YouTube
            };
        }
    }
}

namespace Skybrud.Social.Umbraco.Google.Scope {
    
    internal static class GoogleScopeGroups {
        
        public static readonly GoogleScopeGroup Basic = new GoogleScopeGroup("Basic", new[] {
            new GoogleScopeItem { Alias = "openid", Type = "global", Prefix = "", Name = "OpenID", Description = "" },
            new GoogleScopeItem { Alias = "email", Type = "global", Prefix = "", Name = "Email", Description = "" },
            new GoogleScopeItem { Alias = "profile", Type = "global", Prefix = "", Name = "Profile", Description = "" }
        });

        public static readonly GoogleScopeGroup Analytics = new GoogleScopeGroup("Analytics", new[] {
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/analytics.readonly", Type = "analytics", Prefix = "Analytics: ", Name = "Read-only access", Description = "Read-only access to the Analytics API." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/analytics", Type = "analytics", Prefix = "Analytics: ", Name = "Write access", Description = "Write access to the Analytics API." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/analytics.edit", Type = "analytics", Prefix = "Analytics: ", Name = "Edit", Description = "Edit Google Analytics management entities." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/analytics.manage.users", Type = "analytics", Prefix = "Analytics: ", Name = "Manage users", Description = "	View and manage user permissions for Analytics accounts." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/analytics.manage.users.readonly", Type = "analytics", Prefix = "Analytics: ", Name = "Manage users (readonly)", Description = "View Google Analytics user permissions." }
        });

        public static readonly GoogleScopeGroup YouTube = new GoogleScopeGroup("YouTube", new[] {
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/youtube", Type = "youtube", Prefix = "YouTube: ", Name = "Manage", Description = "Manage your YouTube account." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/youtube.readonly", Type = "youtube", Prefix = "YouTube: ", Name = "Readonly", Description = "View your YouTube account." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/youtube.upload", Type = "youtube", Prefix = "YouTube: ", Name = "Upload", Description = "Upload YouTube videos and manage your YouTube videos." },
            new GoogleScopeItem { Alias = "https://www.googleapis.com/auth/youtubepartner-channel-audit", Type = "youtube", Prefix = "YouTube: ", Name = "Channel audit", Description = "Retrieve the auditDetails part in a channel resource." }
        });
    
    }

    internal class GoogleScopeItem {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }

    internal class GoogleScopeGroup
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("scopes")]
        public GoogleScopeItem[] Scopes { get; private set; }

        public GoogleScopeGroup(string name, GoogleScopeItem[] scopes)
        {
            Name = name;
            Scopes = scopes ?? new GoogleScopeItem[0];
        }
    }
}