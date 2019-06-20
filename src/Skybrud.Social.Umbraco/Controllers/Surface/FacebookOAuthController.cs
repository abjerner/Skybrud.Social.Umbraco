using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Skybrud.Social.Facebook;
using Skybrud.Social.Facebook.Models.Authentication;
using Skybrud.Social.Facebook.Models.Users;
using Skybrud.Social.Facebook.OAuth;
using Skybrud.Social.Facebook.Options.User;
using Skybrud.Social.Mailchimp.Models.Authentication;
using Skybrud.Social.Mailchimp.OAuth;
using Skybrud.Social.Umbraco.Constants;
using Skybrud.Social.Umbraco.Models.Facebook;
using Skybrud.Social.Umbraco.Models.Mailchimp;
using Skybrud.Social.Umbraco.Models.Mvc;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace Skybrud.Social.Umbraco.Controllers.Surface {

    [PluginController(SocialConstants.PluginController)]
    public class FacebookOAuthController : SurfaceController {

        private string _state;

        #region Properties

        /// <summary>
        /// Gets the authorization code from the query string (if specified).
        /// </summary>
        public string AuthCode => Request.QueryString["code"];

        public string AuthState => Request.QueryString["state"];

        public string AuthErrorReason => Request.QueryString["error_reason"];

        public string AuthError => Request.QueryString["error"];

        public bool HasAuthError => String.IsNullOrWhiteSpace(AuthError) == false;

        public string AuthErrorDescription => Request.QueryString["error_description"];

        public string State {
            get => _state ?? Request.QueryString["state"];
            set => _state = value;
        }

        public bool HasState => String.IsNullOrWhiteSpace(State) == false;

        public FacebookOAuthState SessionState => HasState ? Session["Skybrud.Social_" + State] as FacebookOAuthState : null;

        public bool HasSessionState => SessionState != null;

        public string Callback => HasSessionState ? SessionState.Callback : Request.QueryString["callback"];

        #endregion

        #region Public API methods

        public ActionResult Authenticate(string contentTypeAlias, string propertyAlias) {

            // Did we receive an error callback from Google?
            if (HasAuthError) {
                return Error("Authentication failed", "The authentication with Facebook failed. Close this window and try again.");
            }

            // Handle Umbraco authentication stuff
            HttpContextWrapper http = new HttpContextWrapper(System.Web.HttpContext.Current);
            AuthenticationTicket ticket = http.GetUmbracoAuthTicket();

            // User must be logged in
            if (http.AuthenticateCurrentRequest(ticket, true) == false) {
                return Error("Not logged in", "You must be logged in to Umbraco to access this page. Close this window and try again.");
            }

            // Do we have a valid session?
            if (HasState && SessionState == null) {
                return Error("Session expired?", "It seems that your browser session has expired. You can try to close this window and try again.");
            }

            if (HasSessionState) {
                contentTypeAlias = SessionState.ContentTypeAlias;
                propertyAlias = SessionState.PropertyAlias;
            }
            
            IContentType ct = Current.Services.ContentTypeService.Get(contentTypeAlias);
            if (ct == null) return Error("Oooops", "Specified content type was not found.");

            PropertyType property = ct.PropertyTypes.FirstOrDefault(x => x.Alias == propertyAlias);
            if (property == null) return Error("Oooops", "Property not found.");

            IDataType dt = Current.Services.DataTypeService.GetDataType(property.DataTypeId);

            Dictionary<string, object> config = dt.ConfigurationAs<Dictionary<string, object>>();

            config.TryGetValue("clientId", out object clientIdObject);
            config.TryGetValue("clientSecret", out object clientSecretObject);
            config.TryGetValue("redirectUri", out object redirectUriObject);

            string clientId = clientIdObject?.ToString();
            string clientSecret = clientSecretObject?.ToString();
            string redirectUri = redirectUriObject?.ToString();

            if (String.IsNullOrWhiteSpace(clientId)) return Error("Oooops", "Invalid data type configuration.");
            if (String.IsNullOrWhiteSpace(clientSecret)) return Error("Oooops", "Invalid data type configuration.");
            if (String.IsNullOrWhiteSpace(redirectUri)) return Error("Oooops", "Invalid data type configuration.");

            // Initialize a new OAuth client
            FacebookOAuthClient client = new FacebookOAuthClient(clientId, clientSecret, redirectUri);
            
            // Redirect the user to the login dialog
            if (AuthCode == null) {
                
                // Generate a new unique/random state
                State = Guid.NewGuid().ToString();

                // Save the state in the current user session
                Session["Skybrud.Social_" + State] = new FacebookOAuthState {
                    Callback = Callback,
                    ContentTypeAlias = contentTypeAlias,
                    PropertyAlias = propertyAlias
                };

                // Construct the authorization URL
                string url = client.GetAuthorizationUrl(State);

                // Redirect the user
                return Redirect(url);

            }

            // Exchange the authorization code for an access token
            FacebookToken info;
            try {
                info = client.GetAccessTokenFromAuthCode(AuthCode).Body;
            } catch (Exception ex) {
                Logger.Error<FacebookOAuthController>(ex, "Unable to acquire access token.");
                return Error("Authentication failed", "Unable to acquire access token.", ex);
            }
            
            try {

                // Initialize a new Facebook service (no calls are made here)
                FacebookService service = FacebookService.CreateFromAccessToken(info.AccessToken);

                // Make a request to the Facebook API to get information about the user
                FacebookUser user = service.Users.GetUser("me").Body;

                // Set the callback data
                FacebookOAuthData data = new FacebookOAuthData(info, user);

                return View("~/App_Plugins/Skybrud.Social/Views/Facebook/AuthenticateSuccess.cshtml", new FacebookAuthenticatedPageModel {
                    Title = "Auhentication",
                    User = user,
                    Callback = Callback,
                    CallbackData = data
                });

            } catch (Exception ex) {

                Logger.Error<MailchimpOAuthController>(ex, "Unable to get user information.");

                return Error("Authenticated failed", "Unable to get user information.", ex);

            }

        }

        private ActionResult Error(string title, string message, Exception exception = null) {
            return View("~/App_Plugins/Skybrud.Social/Views/AuthenticateError.cshtml", new ErrorPageModel {
                Title = title,
                Message = message,
                Exception = exception
            });
        }

        #endregion

    }

}