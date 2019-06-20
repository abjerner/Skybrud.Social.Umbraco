using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Skybrud.Social.Mailchimp.Models.Authentication;
using Skybrud.Social.Mailchimp.OAuth;
using Skybrud.Social.Umbraco.Constants;
using Skybrud.Social.Umbraco.Models.Mailchimp;
using Skybrud.Social.Umbraco.Models.Mvc;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace Skybrud.Social.Umbraco.Controllers.Surface {

    [PluginController(SocialConstants.PluginController)]
    public class MailchimpOAuthController : SurfaceController {

        private string _state;

        #region Properties

        /// <summary>
        /// Gets the authorization code from the query string (if specified).
        /// </summary>
        public string AuthCode => Request.QueryString["code"];

        /// <summary>
        /// Gets the OAuth <c>state</c> parameter from the query string.
        /// </summary>
        public string AuthState => Request.QueryString["state"];

        /// <summary>
        /// Gets the reason for <see cref="AuthError"/>. This property will only have a valid if the authentication
        /// failed and the user was redirected back to this controller.
        /// </summary>
        public string AuthErrorReason => Request.QueryString["error_reason"];

        /// <summary>
        /// Gets the value of OAuth authentication <c>error</c> parameter. This property will only have a valid if the
        /// authentication failed and the user was redirected back to this controller.
        /// </summary>
        public string AuthError => Request.QueryString["error"];

        /// <summary>
        /// Gets whether the OAuth authentication <c>error</c> parameter is present in the query string.
        /// </summary>
        public bool HasAuthError => string.IsNullOrWhiteSpace(AuthError) == false;

        public string AuthErrorDescription => Request.QueryString["error_description"];

        public string State {
            get => _state ?? Request.QueryString["state"];
            set => _state = value;
        }

        public bool HasState => string.IsNullOrWhiteSpace(State) == false;

        public MailchimpOAuthState SessionState => HasState ? Session["Skybrud.Social_" + State] as MailchimpOAuthState : null;

        public bool HasSessionState => SessionState != null;

        public string Callback => HasSessionState ? SessionState.Callback : Request.QueryString["callback"];

        #endregion

        #region Public API methods

        public ActionResult Authenticate(string contentTypeAlias, string propertyAlias) {

            // Did we receive an error callback from Google?
            if (HasAuthError) {
                return Error("Authentication failed", "The authentication with Mailchimp failed. Close this window and try again.");
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

            if (string.IsNullOrWhiteSpace(clientId)) return Error("Oooops", "Invalid data type configuration.");
            if (string.IsNullOrWhiteSpace(clientSecret)) return Error("Oooops", "Invalid data type configuration.");
            if (string.IsNullOrWhiteSpace(redirectUri)) return Error("Oooops", "Invalid data type configuration.");

            MailchimpOAuthClient client = new MailchimpOAuthClient(clientId, clientSecret, redirectUri);
            
            // Redirect the user to the login dialog
            if (AuthCode == null) {
                
                // Generate a new unique/random state
                State = Guid.NewGuid().ToString();

                // Save the state in the current user session
                Session["Skybrud.Social_" + State] = new MailchimpOAuthState {
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
            MailchimpTokenInfo info;
            try {
                info = client.GetAccessTokenFromAuthCode(AuthCode).Body;
            } catch (Exception ex) {
                Logger.Error<MailchimpOAuthController>(ex, "Unable to acquire access token.");
                return Error("Authentication failed", "Unable to acquire access token.", ex);
            }

            // Update the client with the access token
            client.AccessToken = info.AccessToken;

            try {

                // Get information about the authenticated user
                MailchimpMetadata meta = client.GetMetadata().Body;

                // Set the callback data
                MailchimpOAuthData data = new MailchimpOAuthData(info, meta);

                return View("~/App_Plugins/Skybrud.Social/Views/Mailchimp/AuthenticateSuccess.cshtml", new MailchimpAuthenticatedPageModel {
                    Title = "Auhentication",
                    Meta = meta,
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