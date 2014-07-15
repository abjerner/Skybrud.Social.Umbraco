### Facebook OAuth

This property editor will let an editor (our just you as a developer) attach his/her user information to a page in Umbraco. Brief information about the Facebook user will be saved, but most importantly also an access token, that can be used to make calls to the Facebook API on behalf of the editor/user.

To get started, you must first create a new data type via the developer section, and then choose the property editor named `Skybrud.Social - Facebook OAuth`, and then enter the information of your Facebook app.

If you haven't already created a Facebook app, you can do so via the [Facebook developer section](https://developers.facebook.com/). In order to create an app, you must have verified your Facebook account (they send an SMS, you enter a code). Once you have an app, you can simply fill in the App ID and App Secret as listed on Facebook.

The redirect URI is where the user should be redirected after a successful login. In order for the property to work correctly, this has to be something like `http://yourdomain.com/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx`. You should make sure that this is the same redirect URI that you have specified for your Facebook app.

Now add a property to your document type, and your editors can now attach their Facebook user information.

**Use the property in a view**

The property has value converter, so you can get the OAuth data like the code below:

```C#
// Get the OAuth information stored in the property
FacebookOAuthData facebook = Model.GetPropertyValue("facebook") as FacebookOAuthData;
```

If the given page doesn't have a property of that type, or no information is entered, you can check for that with the code below. The `IsValid` property will validate the access token against the information stored in Umbraco - that is whether the OAuth data has an access token, and that the saved expiry date hasn't been passed. The access token may still be invalid - eg. if the user has deauthorized the app via Facebook.

```C#
// Check whether the OAuth data is valid
if (facebook != null && facebook.IsValid) {

}
```

We now have sufficient information to start making API calls using *Skybrud.Social*. In Skybrud.Social you access the Facebook API via a class called `FacebookService`:

```C#
// Gets an instance of FacebookService based on the OAuth data
FacebookService service = facebook.GetService();
```

Below is an example of a partial view using the `FacebookService` class:

```C#
@using Skybrud.Social.Facebook
@using Skybrud.Social.Facebook.Objects
@using Skybrud.Social.Facebook.Responses
@using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth
@inherits Umbraco.Web.Mvc.UmbracoViewPage
@{

    // Get the OAuth information stored in the property
    FacebookOAuthData facebook = Model.GetPropertyValue("facebook") as FacebookOAuthData;

    // Check whether the OAuth data is valid
    if (facebook != null && facebook.IsValid) {

        // Gets an instance of FacebookService based on the OAuth data
        FacebookService service = facebook.GetService();

        FacebookMeResponse me = service.Methods.Me();
        
        <fieldset>
            <legend>Facebook</legend>
            <p>@me.Name (ID: @me.Id)</p>
        </fieldset>

        // Gets the most recent posts of the authenticated user (me)
        FacebookPostsResponse response = service.Methods.GetPosts("me");
        
        foreach (FacebookPostSummary post in response.Data) {
            
            // Do something with each post
            
        }

    }
    
}
```