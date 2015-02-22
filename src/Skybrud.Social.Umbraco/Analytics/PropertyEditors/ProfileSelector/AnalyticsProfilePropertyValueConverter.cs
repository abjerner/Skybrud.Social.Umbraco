using System;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Social.Umbraco.Analytics.PropertyEditors.ProfileSelector {

    public class AnalyticsProfilePropertyValueConverter : IPropertyValueConverter {

        public bool IsConverter(PublishedPropertyType propertyType) {
            return propertyType.PropertyEditorAlias == "Skybrud.Social.Analytics.ProfileSelector";
        }

        public object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview) {
            return source;
        }

        public object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview) {

            // Get the value as a string
            string str = source as string;

            // Deserialize the string
            return String.IsNullOrWhiteSpace(str) ? null : AnalyticsProfileData.Deserialize(str);

        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview) {
            return null;
        }
    
    }

}