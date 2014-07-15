Skybrud.Social for Umbraco 7
============================
[Skybrud.Social](https://github.com/abjerner/Skybrud.Social) is a framework in .NET that integrates with a number of social services. This repository is for a package for Umbraco 7 that will use Skybrud.Social for bringing stuff like OAuth authentication to Umbraco.

If you're using Umbraco 6, have a look at [this package](https://github.com/abjerner/Skybrud.Social-for-Umbraco)

**Skybrud.Social for Umbraco 7** focuses on the Umbraco backend by adding a set of data types. As of now there are data types to handle the OAuth authentication for the APIs of Facebook, Twitter, Google and Instagram.

![Example of OAuth data types in action](docs/images/readme1.png)

At the moment this package will only help you with the OAuth authentication in the backend, as well as saving the relevant OAuth data in Umbraco. Except some property value converters, there are currently not any code to aid you in the frontend.

Since this package uses [Skybrud.Social](https://github.com/abjerner/Skybrud.Social) in the backend, you might as well use it in the frontend. Under the *Documentation* tab for the [package at Our Umbraco][UmbracoPackageUrl] you will find a view partial views that will help you getting started in the frontend by using Skybrud.Social ;)

### Installation

1. [**NuGet Package**][NuGetPackageUrl]  
Install this NuGet package in your Visual Studio project. Makes updating easy.

2. [**Umbraco package**][UmbracoPackageUrl]  
Install this Umbraco package via the developer section in Umbraco.

3. [**ZIP file**][GitHubReleaseUrl]  
Manually unzip and move files to the root directory of your website.

### Documentation

- Promise to add some documentation ;)

### Roadmap

The first (and current) release of this package only has OAuth data types for a few services: Facebook, Twitter, Google and Instagram. My plan is to add support for other services already supported in Skybrud.Social - and for some that are not currently supported in Skybrud.Social.

This package will also data types for more than just OAuth data. My plans are something along (but not limited to):

- Facebook
  - Group Picker
  - User Picker
  - Post Picker
- Google
  - YouTube
    - Video Picker
    - Playlist Picker
    - Channel Picker
  - Analytics
- Instagram
  - User Picker
- Twitter
  - User Picker
  - Tweet Picker
  - List Picker
- LinkedIn
  - User Picker
  - Group Picker
- GitHub
  - User Picker
  - Repository Picker
  - Gist Picker
- More..

I already have the code for some of these pickers, but need to polish them before they are released. I primarily work on this package in my spare time, so I don't have a timeframe for when what will be added. But if you'd really like to see a feature, please feel free to [contact me via Twitter][TwitterIntent] or [create a new topic at Our Umbraco][OurNewTopic].



[NuGetPackageUrl]: https://www.nuget.org/packages/Skybrud.Social.Umbraco/1.0.0
[UmbracoPackageUrl]: http://our.umbraco.org/projects/website-utilities/skybrudsocial-for-umbraco-7
[GitHubReleaseUrl]: https://github.com/abjerner/Skybrud.Social.Umbraco/releases/latest
[TwitterIntent]: https://twitter.com/intent/tweet?screen_name=abjerner&text=Hey%20there.%20If%20I%27ll%20give%20you%20my%20first%20born,%20will%20you...
[OurNewTopic]: http://our.umbraco.org/projects/website-utilities/skybrudsocial-for-umbraco-7/general-discussion




