using System;
using Skybrud.Social.Facebook.Models.Users;
using Skybrud.Social.Mailchimp.Models.Authentication;

namespace Skybrud.Social.Umbraco.Models.Mvc {

    public class PageModel {

        public string Title { get; set; }

    }

    public class ErrorPageModel : PageModel {

        public string Message { get; set; }

        public Exception Exception { get; set; }

    }
    
    public class FacebookAuthenticatedPageModel : PageModel {
            
        public FacebookUser User { get; set; }

        public string Callback { get; set; }

        public object CallbackData { get; set; }

    }
    
    public class MailchimpAuthenticatedPageModel : PageModel {
            
        public MailchimpMetadata Meta { get; set; }

        public string Callback { get; set; }

        public object CallbackData { get; set; }

    }

    //public class GoogleAuthenticatedPageModel : PageModel {

    //    public string Callback { get; set; }

    //    public GoogleOAuthData CallbackData { get; set; }

    //}

}