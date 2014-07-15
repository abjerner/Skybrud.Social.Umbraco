using System;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Skybrud.Social.Google;
using Skybrud.Social.Google.OAuth;
using Skybrud.Social.Umbraco.Google;
using Skybrud.Social.Umbraco.Google.PropertyEditors.OAuth;

namespace Skybrud.Social.Umbraco.App_Plugins.Skybrud.Social.Dialogs {

    public partial class GoogleOAuth : System.Web.UI.Page {

        #region Umbraco related properties
        
        public string Callback { get; private set; }

        public string ContentTypeAlias { get; private set; }

        public string PropertyAlias { get; private set; }

        public string Feature { get; private set; }

        #endregion

        /// <summary>
        /// Gets the authorizing code from the query string (if specified).
        /// </summary>
        public string AuthCode {
            get { return Request.QueryString["code"]; }
        }

        public string AuthState {
            get { return Request.QueryString["state"]; }
        }

        public string AuthErrorReason {
            get { return Request.QueryString["error_reason"]; }
        }

        public string AuthError {
            get { return Request.QueryString["error"]; }
        }

        public string AuthErrorDescription {
            get { return Request.QueryString["error_description"]; }
        }

        protected void Page_Load(object sender, EventArgs e) {

            Title = "Google OAuth";

            Callback = Request.QueryString["callback"];
            ContentTypeAlias = Request.QueryString["contentTypeAlias"];
            PropertyAlias = Request.QueryString["propertyAlias"];
            Feature = Request.QueryString["feature"];

            if (AuthState != null) {
                NameValueCollection stateValue = Session["Skybrud.Social_" + AuthState] as NameValueCollection;
                if (stateValue != null) {
                    Callback = stateValue["Callback"];
                    ContentTypeAlias = stateValue["ContentTypeAlias"];
                    PropertyAlias = stateValue["PropertyAlias"];
                    Feature = stateValue["Feature"];
                }
            }

            // Get the prevalue options
            GoogleOAuthPreValueOptions options = GoogleOAuthPreValueOptions.Get(ContentTypeAlias, PropertyAlias);
            if (!options.IsValid) {
                Content.Text += "Hold on now! The options of the underlying prevalue editor isn't valid.";
                return;
            }

            // Configure the OAuth client based on the options of the prevalue options
            GoogleOAuthClient client = new GoogleOAuthClient {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                RedirectUri = options.RedirectUri
            };

            // Session expired?
            if (AuthState != null && Session["Skybrud.Social_" + AuthState] == null) {
                Content.Text = "<div class=\"error\">Session expired?</div>";
                return;
            }

            // Check whether an error response was received from Google
            if (AuthError != null) {
                Content.Text = "<div class=\"error\">Error: " + AuthErrorDescription + "</div>";
                if (AuthState != null) Session.Remove("Skybrud.Social:" + AuthState);
                return;
            }

            string state;

            // Redirect the user to the Google login dialog
            if (AuthCode == null) {

                // Generate a new unique/random state
                state = Guid.NewGuid().ToString();

                // Save the state in the current user session
                Session["Skybrud.Social_" + state] = new NameValueCollection {
                    { "Callback", Callback},
                    { "ContentTypeAlias", ContentTypeAlias},
                    { "PropertyAlias", PropertyAlias},
                    { "Feature", Feature}
                };

                // Declare the scope
                string[] scope = new[] {
                    GoogleScope.OpenId,
                    GoogleScope.Email,
                    GoogleScope.Profile
                };

                // Construct the authorization URL
                string url = client.GetAuthorizationUrl(state, String.Join(" ", scope), GoogleAccessType.Offline, GoogleApprovalPrompt.Force);
                
                // Redirect the user
                Response.Redirect(url);
                return;
            
            }

            GoogleAccessTokenResponse info;
            try {
                info = client.GetAccessTokenFromAuthorizationCode(AuthCode);
            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to acquire access token</b><br />" + ex.Message + "</div>";
                return;
            }

            try {

                // Initialize the Google service
                GoogleService service = GoogleService.CreateFromRefreshToken(client.ClientIdFull, client.ClientSecret, info.RefreshToken);

                // Get information about the authenticated user
                GoogleUserInfo user = service.GetUserInfo();
                
                Content.Text += "<p>Hi <strong>" + user.Name + "</strong></p>";
                Content.Text += "<p>Please wait while you're being redirected...</p>";

                // Set the callback data
                GoogleOAuthData data = new GoogleOAuthData {
                    Id = user.Id,
                    Name = user.Name,
                    Avatar = user.Picture,
                    ClientId = client.ClientIdFull,
                    ClientSecret = client.ClientSecret,
                    RefreshToken = info.RefreshToken
                };

                // Update the UI and close the popup window
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                    "self.opener." + Callback + "({0}); window.close();",
                    data.Serialize()
                ), true);

            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to get user information</b><br />" + ex.Message + "</div>";
                return;
            }

        }
    
    }

}