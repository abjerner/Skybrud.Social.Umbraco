using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using Skybrud.Social.Facebook;
using Skybrud.Social.Facebook.OAuth;
using Skybrud.Social.Facebook.Objects.Debug;
using Skybrud.Social.Facebook.Objects.Users;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core.Security;
using Skybrud.Social.Facebook.Responses.Accounts;
using Skybrud.Social.Facebook.Objects.Accounts;

namespace Skybrud.Social.Umbraco.App_Plugins.Skybrud.Social.Dialogs {
    
    public partial class FacebookOAuth : System.Web.UI.Page {

        public string Callback { get; private set; }

        public string ContentTypeAlias { get; private set; }

        public string PropertyAlias { get; private set; }

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
            FacebookOAuthPreValueOptions options = FacebookOAuthPreValueOptions.Get(ContentTypeAlias, PropertyAlias);
            if (!options.IsValid) {
                Content.Text = "Hold on now! The options of the underlying prevalue editor isn't valid.";
                return;
            }

            // Configure the OAuth client based on the options of the prevalue options
            FacebookOAuthClient client = new FacebookOAuthClient {
                AppId = options.AppId,
                AppSecret = options.AppSecret,
                RedirectUri = options.RedirectUri

//TODO: Interferir aqui pra add multiplas permissões
            };

            // Session expired?
            if (AuthState != null && Session["Skybrud.Social_" + AuthState] == null) {
                Content.Text = "<div class=\"error\">Session expired?</div>";
                return;
            }

            // Check whether an error response was received from Facebook
            if (AuthError != null) {
                Content.Text = "<div class=\"error\">Error: " + AuthErrorDescription + "</div>";
                return;
            }

            // Redirect the user to the Facebook login dialog
            if (AuthCode == null) {

                // Generate a new unique/random state
                string state = Guid.NewGuid().ToString();

                // Save the state in the current user session
                Session["Skybrud.Social_" + state] = new[] {Callback, ContentTypeAlias, PropertyAlias};

                // Construct the authorization URL
                string url = client.GetAuthorizationUrl(state, "manage_pages",/*"read_stream",*/ "user_status", "user_about_me", "user_photos");
                
                // Redirect the user
                Response.Redirect(url);
                return;
            
            }

            // Exchange the authorization code for a user access token
            string userAccessToken;
            try {
                userAccessToken = client.GetAccessTokenFromAuthCode(AuthCode);
            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to acquire access token</b><br />" + ex.Message + "</div>";
                return;
            }

            //try {

                // Initialize the Facebook service (no calls are made here)
                FacebookService service = FacebookService.CreateFromAccessToken(userAccessToken);

                // Make a call to the Facebook API to get information about the user
                FacebookUser me = service.Users.GetUser("me").Body;




            //DIogo:
//Funciona só com permissão "manage_pages"
        // Make the request to the API
        FacebookAccountsResponse response = service.Accounts.GetAccounts();

        // Loop through the accounts
        foreach (FacebookAccount account in response.Body.Data)
        {
            // Write some of the account information to the console
            Trace.Write("Name: " + account.Name);
            Trace.Write("Category: " + account.Category);
            Trace.Write("Token: " + account.AccessToken);
            Trace.Write(Environment.NewLine);
            Trace.Write(Environment.NewLine);
        }





                // Get debug information about the access token
                FacebookDebugToken debug = service.Debug.DebugToken(userAccessToken).Body;

                Content.Text += "<p>Hi <strong>" + me.Name + "</strong></p>";
                Content.Text += "<p>Please wait while you're being redirected...</p>";

                // Set the callback data
                FacebookOAuthData data = new FacebookOAuthData {
                    Id = me.Id,
                    Name = me.Name,
                    AccessToken = userAccessToken,
                    ExpiresAt = debug.Data.ExpiresAt == null ? default(DateTime) : debug.Data.ExpiresAt.Value,
                    Scope = (
                        from scope in debug.Data.Scopes select scope.Name
                    ).ToArray(),

                    BusinessPages = response.Body.Data.
                    Select(ac => new FacebookBusinessPageData() { 
                        Id = ac.Id, Name = ac.Name, AccessToken = ac.AccessToken 
                    }).ToArray(),
                    SelectedBusinessPage = null
                };








                // Update the UI and close the popup window
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                    "self.opener." + Callback + "({0}); window.close();",
                    data.Serialize()//Diogo: salva o JSON via callback x no AngularJS!
                ), true);

            //} catch (Exception ex) {
               // Content.Text = "<div class=\"error\"><b>Unable to get user information</b><br />" + ex.Message + "</div>";
                return;
            //}

        }
    
    }

}