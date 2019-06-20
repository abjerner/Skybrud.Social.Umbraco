using System;
using Skybrud.Social.Umbraco.Models.Mailchimp;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Social.Umbraco.DataTypes {

    class MailchimpOAuthDataPropertyValueConverter : PropertyValueConverterBase {

        public override bool IsConverter(PublishedPropertyType propertyType) {

            switch (propertyType.EditorAlias) {

                case "Skybrud.Social.GoogleOAuth":
                case "Skybrud.Social.MailchimpOAuth":
                    return true;

                default:
                    return false;

            }

        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, PublishedPropertyType propertyType, object source, bool preview) {

            switch (propertyType.EditorAlias) {

                //case "Skybrud.Social.GoogleOAuth":
                //    return GoogleOAuthData.Deserialize(source as string);

                case "Skybrud.Social.MailchimpOAuth":
                    return MailchimpOAuthData.Deserialize(source as string);

                default:
                    return null;

            }

        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return inter;
        }

        public override object ConvertIntermediateToXPath(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return null;
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType) => PropertyCacheLevel.None;

        public override Type GetPropertyValueType(PublishedPropertyType propertyType) {

            switch (propertyType.EditorAlias) {

                //case "Skybrud.Social.GoogleOAuth":
                //    return typeof(GoogleOAuthData);

                case "Skybrud.Social.MailchimpOAuth":
                    return typeof(MailchimpOAuthData);

                default:
                    return typeof(object);

            }

        }

    }

}