namespace Skybrud.Social.Umbraco {
    
    public static class PackageHelpers {

        public static string UmbracoVersion {
            get { return global::Umbraco.Core.Configuration.UmbracoVersion.Current.ToString(); }
        }

    }

}