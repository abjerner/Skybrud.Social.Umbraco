using System;
using System.Web;
using System.Web.Security;
using Skybrud.Social.Instagram;
using Skybrud.Social.Instagram.OAuth;
using Skybrud.Social.Instagram.Objects;
using Skybrud.Social.Instagram.Responses;
using Skybrud.Social.Umbraco.Instagram.PropertyEditors.OAuth;
using Umbraco.Core.Security;

namespace Skybrud.Social.Umbraco.App_Plugins.Skybrud.Social.Dialogs {

    public partial class InstagramOAuth : System.Web.UI.Page {

        #region Umbraco related properties
        
        public string Callback { get; private set; }

        public string ContentTypeAlias { get; private set; }

        public string PropertyAlias { get; private set; }

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

        protected override void OnPreInit(EventArgs e) {

            base.OnPreInit(e);

            if (PackageHelpers.UmbracoVersion != "7.2.2") return;

            // Handle authentication stuff to counteract bug in Umbraco 7.2.2 (see U4-6342)
            HttpContextWrapper http = new HttpContextWrapper(Context);
            FormsAuthenticationTicket ticket = http.GetUmbracoAuthTicket();
            http.AuthenticateCurrentRequest(ticket, true);

        }

        protected void Page_Load(object sender, EventArgs e) {

            Callback = Request.QueryString["callback"];
            ContentTypeAlias = Request.QueryString["contentTypeAlias"];
            PropertyAlias = Request.QueryString["propertyAlias"];

            if (AuthState != null) {
                string[] stateValue = Session["Skybrud.Social_" + AuthState] as string[];
                if (stateValue != null && stateValue.Length == 3) {
                    Callback = stateValue[0];
                    ContentTypeAlias = stateValue[1];
                    PropertyAlias = stateValue[2];
                }
            }

            // Get the prevalue options
            InstagramOAuthPreValueOptions options = InstagramOAuthPreValueOptions.Get(ContentTypeAlias, PropertyAlias);
            if (!options.IsValid) {
                Content.Text = "Hold on now! The options of the underlying prevalue editor isn't valid.";
                return;
            }

            // Configure the OAuth client based on the options of the prevalue options
            InstagramOAuthClient client = new InstagramOAuthClient {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                RedirectUri = options.RedirectUri
            };

            // Session expired?
            if (AuthState != null && Session["Skybrud.Social_" + AuthState] == null) {
                Content.Text = "<div class=\"error\">Session expired?</div>";
                return;
            }

            // Check whether an error response was received from Instagram
            if (AuthError != null) {
                Content.Text = "<div class=\"error\">Error: " + AuthErrorDescription + "</div>";
                return;
            }

            // Redirect the user to the Instagram login dialog
            if (AuthCode == null) {

                // Generate a new unique/random state
                string state = Guid.NewGuid().ToString();

                // Save the state in the current user session
                Session["Skybrud.Social_" + state] = new[] { Callback, ContentTypeAlias, PropertyAlias };

                // Construct the authorization URL
                string url = client.GetAuthorizationUrl(state);
                
                // Redirect the user
                Response.Redirect(url);
                return;
            
            }
            
            // Exchange the authorization code for an access token
            InstagramAccessTokenResponse accessToken;
            try {
                accessToken = client.GetAccessTokenFromAuthCode(AuthCode);
            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to acquire access token</b><br />" + ex.Message + "</div>";
                return;
            }

            try {

                // Initialize the Instagram service
                InstagramService service = InstagramService.CreateFromAccessToken(accessToken.Body.AccessToken);

                // Get information about the authenticated user
                InstagramUser user = service.Users.GetSelf().Body.Data;



                Content.Text += "<p>Hi <strong>" + (user.FullName ?? user.Username) + "</strong></p>";
                Content.Text += "<p>Please wait while you're being redirected...</p>";

                // Set the callback data
                InstagramOAuthData data = new InstagramOAuthData {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    Name = user.FullName ?? user.Username,
                    Avatar = user.ProfilePicture,
                    AccessToken = accessToken.Body.AccessToken
                };

                // Update the UI and close the popup window
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                    "self.opener." + Callback + "({0}); window.close();",
                    data.Serialize()
                ), true);

            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to get user information</b><br />" + ex.Message + "</div>";
            }

        }
    
    }

}