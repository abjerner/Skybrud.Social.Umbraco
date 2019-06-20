//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.Owin.Security;
//using Newtonsoft.Json.Linq;
//using Skybrud.Social.Google;
//using Skybrud.Social.Google.Models;
//using Skybrud.Social.Google.Models.Authentication;
//using Skybrud.Social.Google.OAuth;
//using Skybrud.Social.Google.Scopes;
//using Skybrud.Social.Umbraco.Constants;
//using Skybrud.Social.Umbraco.Models.Google;
//using Skybrud.Social.Umbraco.Models.Mvc;
//using Umbraco.Core.Composing;
//using Umbraco.Core.Logging;
//using Umbraco.Core.Models;
//using Umbraco.Web.Mvc;
//using Umbraco.Web.Security;

//namespace Skybrud.Social.Umbraco.Controllers.Surface {

//    [PluginController(SocialConstants.PluginController)]
//    public class GoogleOAuthController : SurfaceController {

//        private string _state;

//        #region Properties

//        /// <summary>
//        /// Gets the authorization code from the query string (if specified).
//        /// </summary>
//        public string AuthCode => Request.QueryString["code"];

//        public string AuthState => Request.QueryString["state"];

//        public string AuthErrorReason => Request.QueryString["error_reason"];

//        public string AuthError => Request.QueryString["error"];

//        public bool HasAuthError => String.IsNullOrWhiteSpace(AuthError) == false;

//        public string AuthErrorDescription => Request.QueryString["error_description"];

//        public string State {
//            get => _state ?? Request.QueryString["state"];
//            set => _state = value;
//        }

//        public bool HasState => String.IsNullOrWhiteSpace(State) == false;

//        public GoogleOAuthState SessionState => HasState ? Session["Skybrud.Social_" + State] as GoogleOAuthState : null;

//        public bool HasSessionState => SessionState != null;

//        public string Callback => HasSessionState ? SessionState.Callback : Request.QueryString["callback"];

//        #endregion

//        #region Public API methods

//        public ActionResult Authenticate(string contentTypeAlias, string propertyAlias) {

//            // Did we receive an error callback from Google?
//            if (HasAuthError) {
//                return Error("Authentication failed", "The authentication with Google failed. Close this window and try again.");
//            }

//            // Handle Umbraco authentication stuff
//            HttpContextWrapper http = new HttpContextWrapper(System.Web.HttpContext.Current);
//            AuthenticationTicket ticket = http.GetUmbracoAuthTicket();

//            // User must be logged in
//            if (http.AuthenticateCurrentRequest(ticket, true) == false) {
//                return Error("Not logged in", "You must be logged in to Umbraco to access this page. Close this window and try again.");
//            }

//            // Do we have a valid session?
//            if (HasState && SessionState == null) {
//                return Error("Session expired?", "It seems that your browser session has expired. You can try to close this window and try again.");
//            }

//            if (HasSessionState) {
//                contentTypeAlias = SessionState.ContentTypeAlias;
//                propertyAlias = SessionState.PropertyAlias;
//            }
            
//            IContentType ct = Current.Services.ContentTypeService.Get(contentTypeAlias);
//            if (ct == null) return Error("Oooops", "Specified content type was not found.");

//            PropertyType property = ct.PropertyTypes.FirstOrDefault(x => x.Alias == propertyAlias);
//            if (property == null) return Error("Oooops", "Property not found.");

//            IDataType dt = Current.Services.DataTypeService.GetDataType(property.DataTypeId);

//            Dictionary<string, object> config = dt.ConfigurationAs<Dictionary<string, object>>();

//            config.TryGetValue("clientId", out object clientIdObject);
//            config.TryGetValue("clientSecret", out object clientSecretObject);
//            config.TryGetValue("redirectUri", out object redirectUriObject);
//            config.TryGetValue("scopes", out object scopes);

//            GoogleScopeCollection scope = new GoogleScopeCollection();

//            if (scopes is JArray) {
//                foreach (string alias in ((JArray) scopes).Select(x => x.ToString())) {
//                    if (GoogleUtils.TryParseScope(alias, out GoogleScope s)) scope.Add(s);
//                }
//            }

//            string clientId = clientIdObject?.ToString();
//            string clientSecret = clientSecretObject?.ToString();
//            string redirectUri = redirectUriObject?.ToString();

//            if (String.IsNullOrWhiteSpace(clientId)) return Error("Oooops", "Invalid data type configuration. No client ID specified.");
//            if (String.IsNullOrWhiteSpace(clientSecret)) return Error("Oooops", "Invalid data type configuration. No client secret specified.");
//            if (String.IsNullOrWhiteSpace(redirectUri)) return Error("Oooops", "Invalid data type configuration. No redirect URI specified.");
//            if (scope.Items.Length == 0) return Error("Oooops", "Invalid data type configuration. One or more scopes must be selected.");

//            GoogleOAuthClient client = new GoogleOAuthClient(clientId, clientSecret, redirectUri);
            
//            // Redirect the user to the login dialog
//            if (AuthCode == null) {
                
//                // Generate a new unique/random state
//                State = Guid.NewGuid().ToString();

//                // Save the state in the current user session
//                Session["Skybrud.Social_" + State] = new GoogleOAuthState {
//                    Callback = Callback,
//                    ContentTypeAlias = contentTypeAlias,
//                    PropertyAlias = propertyAlias
//                };

//                // Construct the authorization URL
//                string url = client.GetAuthorizationUrl(State, scope, GoogleAccessType.Offline, GoogleApprovalPrompt.Force);

//                // Redirect the user
//                return Redirect(url);

//            }

//            // Exchange the authorization code for an access token
//            GoogleToken info;
//            try {
//                info = client.GetAccessTokenFromAuthorizationCode(AuthCode).Body;
//            } catch (Exception ex) {
//                Logger.Error<GoogleOAuthController>(ex, "Unable to acquire access token.");
//                return Error("Authentication failed", "Unable to acquire access token.", ex);
//            }
            
//            // Update the client with the access token
//            client.AccessToken = info.AccessToken;

//            try {

//                // Initialize the Google service
//                GoogleService service = GoogleService.CreateFromRefreshToken(client.ClientId, client.ClientSecret, info.RefreshToken);
                
//                // Get information about the authenticated user
//                GoogleUserInfo user = service.GetUserInfo().Body;

//                return View("~/App_Plugins/Skybrud.Social/Views/Google/AuthenticateSuccess.cshtml", new GoogleAuthenticatedPageModel {
//                    Title = "Auhentication",
//                    Callback = Callback,
//                    CallbackData = new GoogleOAuthData(client.ClientId, client.ClientSecret, info, user)
//                });

//            } catch (Exception ex) {

//                Logger.Error<GoogleOAuthController>(ex, "Unable to get user information.");

//                return Error("Authenticated failed", "Unable to get user information.", ex);

//            }

//        }

//        private ActionResult Error(string title, string message, Exception exception = null) {
//            return View("~/App_Plugins/Skybrud.Social/Views/AuthenticateError.cshtml", new ErrorPageModel {
//                Title = title,
//                Message = message,
//                Exception = exception
//            });
//        }

//        #endregion

//    }

//}