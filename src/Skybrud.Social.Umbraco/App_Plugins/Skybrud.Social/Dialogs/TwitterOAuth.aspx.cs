using System;
using System.Web;
using System.Web.Security;
using Skybrud.Social.OAuth;
using Skybrud.Social.Twitter;
using Skybrud.Social.Twitter.Exceptions;
using Skybrud.Social.Twitter.OAuth;
using Skybrud.Social.Twitter.Objects;
using Skybrud.Social.Umbraco.Twitter.PropertyEditors.OAuth;
using Umbraco.Core.Security;

namespace Skybrud.Social.Umbraco.App_Plugins.Skybrud.Social.Dialogs {

    public partial class TwitterOAuth : System.Web.UI.Page {

        #region Properties

        public string Callback { get; private set; }

        public string ContentTypeAlias { get; private set; }

        public string PropertyAlias { get; private set; }

        #region OAuth stuff

        /// <summary>
        /// Gets the authorizing code from the query string (if specified).
        /// </summary>
        public string OAuthToken {
            get { return Request.QueryString["oauth_token"]; }
        }

        public string OAuthVerifier {
            get { return Request.QueryString["oauth_verifier"]; }
        }

        public string DeniedToken {
            get { return Request.QueryString["denied"]; }
        }

        public bool HasUserDenied {
            get { return !String.IsNullOrEmpty(DeniedToken); }
        }

        #endregion

        #endregion

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

            // Get the prevalue options
            TwitterOAuthPreValueOptions options = TwitterOAuthPreValueOptions.Get(ContentTypeAlias, PropertyAlias);
            if (!options.IsValid) {
                Content.Text += "<p><strong>ContentTypeAlias</strong> " + ContentTypeAlias + "</p>";
                Content.Text += "<p><strong>PropertyAlias</strong> " + PropertyAlias + "</p>";
                Content.Text += "Hold on now! The options of the underlying prevalue editor isn't valid.";
                return;
            }

            // Configure the OAuth client based on the options of the prevalue options
            TwitterOAuthClient client = new TwitterOAuthClient {
                ConsumerKey = options.ConsumerKey,
                ConsumerSecret = options.ConsumerSecret,
                Callback = Request.Url.ToString()
            };
            
            // Check whether the user has denied the app
            if (HasUserDenied) {
                Session.Remove(DeniedToken);
                Content.Text = "<div class=\"error\">Error: The app was denied access to your account.</div>";
                return;
            }

            #region OAuth 1.0a - Step 3

            if (OAuthToken != null) {
                
                // Grab the request token from the session
                OAuthRequestToken token = Session[OAuthToken] as OAuthRequestToken;

                // Check whether the requets token was found in the current session
                if (token == null) {
                    Content.Text = "<div class=\"error\">An error occured. Timeout?</div>";
                    return;
                }

                // Update the token and token secret
                client.Token = token.Token;
                client.TokenSecret = token.TokenSecret;

                // Now get the access token
                try {
                    OAuthAccessToken accessToken = client.GetAccessToken(OAuthVerifier);
                    client.Token = accessToken.Token;
                    client.TokenSecret = accessToken.TokenSecret;
                } catch (Exception) {
                    Content.Text = "<div class=\"error\">Unable to retrieve access token from <b>Twitter.com</b>.</div>";
                    return;
                }

                try {

                    // Initialize an instance of TwitterService
                    TwitterService service = TwitterService.CreateFromOAuthClient(client);

                    // Get information about the server
                    TwitterUser user = service.Account.VerifyCredentials().Body;

                    Content.Text += "<p>Hi <strong>" + (String.IsNullOrEmpty(user.Name) ? user.ScreenName : user.Name) + "</strong></p>";
                    Content.Text += "<p>Please wait while you're being redirected...</p>";

                    // Set the callback data
                    TwitterOAuthData data = new TwitterOAuthData {
                        Id = user.Id,
                        ScreenName = user.ScreenName,
                        Name = String.IsNullOrEmpty(user.Name) ? "" : user.Name,
                        Avatar = user.ProfileImageUrlHttps,
                        ConsumerKey = client.ConsumerKey,
                        ConsumerSecret = client.ConsumerSecret,
                        AccessToken = client.Token,
                        AccessTokenSecret = client.TokenSecret
                    };

                    // Update the UI and close the popup window
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                        "self.opener." + Callback + "({0}); window.close();",
                        data.Serialize()
                    ), true);

                } catch (TwitterException ex) {
                    Content.Text = "<div class=\"error\">Error in the communication with Twitter.com<br /><br />" + ex.Message + " (Code: " + ex.Code + ")</div>";
                } catch (Exception) {
                    Content.Text = "<div class=\"error\">Error in the communication with Twitter.com</div>";
                }

                return;

            }

            #endregion

            #region OAuth 1.0a - Step 1

            // Get a request token from the Twitter API
            OAuthRequestToken requestToken = client.GetRequestToken();

            // Save the token information to the session so we can grab it later
            Session[requestToken.Token] = requestToken;
            
            // Redirect the user to the authentication page at Twitter.com
            Response.Redirect(requestToken.AuthorizeUrl);

            #endregion

        }
    
    }

}