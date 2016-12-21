using Newtonsoft.Json;
using Skybrud.Social.Google.Analytics;
using Skybrud.Social.Google.OAuth;
using Skybrud.Social.Google.YouTube;
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

namespace Skybrud.Social.Umbraco.Google.Scope
{
    internal static class GoogleScopeGroups
    {
        public static readonly GoogleScopeGroup Basic = new GoogleScopeGroup("Basic", new[] {
            GoogleScopes.Email,
            GoogleScopes.OpenId,
            GoogleScopes.Profile
        });

        public static readonly GoogleScopeGroup Analytics = new GoogleScopeGroup("Analytics", new[] {
            AnalyticsScopes.Readonly,
            AnalyticsScopes.Write,
            AnalyticsScopes.Edit,
            AnalyticsScopes.ManageUsers,
            AnalyticsScopes.ManageUsersReadonly,
        });

        public static readonly GoogleScopeGroup YouTube = new GoogleScopeGroup("YouTube", new[] {
            YouTubeScopes.Readonly,
            YouTubeScopes.Manage,
            YouTubeScopes.Upload,
            YouTubeScopes.PartnerChannelAudit
        });
    }

    internal class GoogleScopeGroup
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("scopes")]
        public GoogleScope[] Scopes { get; private set; }

        public GoogleScopeGroup(string name, GoogleScope[] scopes)
        {
            Name = name;
            Scopes = scopes ?? new GoogleScope[0];
        }
    }
}