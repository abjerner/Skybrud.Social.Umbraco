//using Skybrud.Social.Facebook.Scope;

using Newtonsoft.Json;
using Skybrud.Social.Facebook.Scope;
using Skybrud.Social.Umbraco.WebApi;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Social.Umbraco.Controllers.Api {

    [PluginController("SkybrudSocial")]
    [JsonOnlyConfiguration]
    public class FacebookController : UmbracoApiController {

        public object GetScopes() {
            return new[] {
                FacebookScopeGroups.Basic,
                FacebookScopeGroups.ExtendedProfileProperties,
                FacebookScopeGroups.ExtendedPermissions
            };
        }

    }

}

namespace Skybrud.Social.Facebook.Scope {

    /// <summary>
    /// Static class containing references to groups of scopes of the Facebook API.
    /// </summary>
    internal static class FacebookScopeGroups {

        #region Readonly properties

        /// <summary>
        /// Gets a reference to all scopes currently implemented in Skybrud.Social.
        /// </summary>
        public static readonly FacebookScopeGroup All = new FacebookScopeGroup("All", new[] {
            FacebookScopes.PublicProfile,
            FacebookScopes.Email,
            FacebookScopes.UserFriends,
            FacebookScopes.UserBirthday,
            FacebookScopes.UserHometown,
            FacebookScopes.UserLikes,
            FacebookScopes.UserLocation,
            FacebookScopes.UserManagedGroups,
            FacebookScopes.UserPhotos,
            FacebookScopes.UserPosts,
            FacebookScopes.UserStatus,
            FacebookScopes.UserVideos,
            FacebookScopes.ManagePages,
            FacebookScopes.PublishPages,
            FacebookScopes.PublishActions
        });

        /// <summary>
        /// Gets a reference to the most basic scopes of the Facebook API.
        /// </summary>
        public static readonly FacebookScopeGroup Basic = new FacebookScopeGroup("Basic", new[] {
            FacebookScopes.PublicProfile,
            FacebookScopes.Email
        });

        public static readonly FacebookScopeGroup ExtendedProfileProperties = new FacebookScopeGroup("Extended Profile Properties", new[] {
            FacebookScopes.UserFriends,
            FacebookScopes.UserBirthday,
            FacebookScopes.UserHometown,
            FacebookScopes.UserLikes,
            FacebookScopes.UserLocation,
            FacebookScopes.UserManagedGroups,
            FacebookScopes.UserPhotos,
            FacebookScopes.UserPosts,
            FacebookScopes.UserStatus,
            FacebookScopes.UserVideos
        });

        public static readonly FacebookScopeGroup ExtendedPermissions = new FacebookScopeGroup("Extended Permissions", new[] {
            FacebookScopes.ManagePages,
            FacebookScopes.PublishPages,
            FacebookScopes.PublishActions
        });

        #endregion

    }

    internal class FacebookScopeGroup {

        #region Properties

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets an array of all scopes of the group.
        /// </summary>
        [JsonProperty("scopes")]
        public FacebookScope[] Scopes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new group based on the specified <code>name</code> and <code>scopes</code>.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="scopes">The array of scopes.</param>
        public FacebookScopeGroup(string name, FacebookScope[] scopes) {
            Name = name;
            Scopes = scopes ?? new FacebookScope[0];
        }

        #endregion

    }

    /// <summary>
    /// Enum class describing whether a given scope requires your app to be reviewed by Facebook.
    /// </summary>
    internal enum FacebookScopeReview {

        /// <summary>
        /// Indicates that whether a review is required hasn't been specified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Indicates that a given scope doesn't require review.
        /// </summary>
        No,

        /// <summary>
        /// Indicates that a given scope requires your app to be reviewed by Facebook.
        /// </summary>
        Yes,

    }

    /// <summary>
    /// Static class containing references to scopes of the Facebook API.
    /// </summary>
    internal static class FacebookScopes {

        #region Readonly properties

        /// <summary>
        /// Grants access to basic fields of the public profile of the authenticated user.
        /// </summary>
        /// <see>
        ///     <cref>https://developers.facebook.com/docs/facebook-login/permissions/v2.4#reference-public_profile</cref>
        /// </see>
        public static readonly FacebookScope PublicProfile = new FacebookScope(
            "public_profile",
            "Grants access to basic fields of the public profile of the authenticated user."
        );

        /// <summary>
        /// Grants access to the email address of the authenticated user. Use of the email address
        /// should comply to Facebook's Terms of Service.
        /// </summary>
        public static readonly FacebookScope Email = new FacebookScope(
            "email",
            "Grants access to the email address of the authenticated user. Use of the email address should comply to Facebook's Terms of Service."
        );

        /// <summary>
        /// Grants access to a list of friends of the authenticated user also using the calling app.
        /// </summary>
        public static readonly FacebookScope UserFriends = new FacebookScope(
            "user_friends",
            "Grants access to a list of friends of the authenticated user also using the calling app."
        );

        /// <summary>
        /// Provides access to a person's personal description (the 'About Me' section on their Profile) through the <code>bio</code> property on the User object.
        /// </summary>
        public static readonly FacebookScope UserAboutMe = new FacebookScope(
            "user_about_me",
            "Provides access to a person's personal description (the 'About Me' section on their Profile) through the bio property on the User object."
        );


        
        
        public static readonly FacebookScope UserBirthday = new FacebookScope("user_birthday");
        public static readonly FacebookScope UserHometown = new FacebookScope("user_hometown");
        public static readonly FacebookScope UserLikes = new FacebookScope("user_likes");
        public static readonly FacebookScope UserLocation = new FacebookScope("user_location");
        public static readonly FacebookScope UserManagedGroups = new FacebookScope("user_managed_groups");
        public static readonly FacebookScope UserPhotos = new FacebookScope("user_photos");
        public static readonly FacebookScope UserPosts = new FacebookScope("user_posts");
        public static readonly FacebookScope UserStatus = new FacebookScope("user_status");
        public static readonly FacebookScope UserVideos = new FacebookScope("user_videos");

        public static readonly FacebookScope ManagePages = new FacebookScope("manage_pages");
        public static readonly FacebookScope PublishPages = new FacebookScope("publish_pages");
        public static readonly FacebookScope PublishActions = new FacebookScope("publish_actions");

        #endregion

    }

}