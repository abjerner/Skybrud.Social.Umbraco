using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Skybrud.Social.Umbraco {

    /// <summary>
    /// Static class with various helper methods for pre values.
    /// </summary>
    public static class PreValueHelpers {

        internal static IContentTypeService ContentTypeService {
            get { return UmbracoContext.Current.Application.Services.ContentTypeService; }
        }

        internal static IDataTypeService DataTypeService {
            get { return UmbracoContext.Current.Application.Services.DataTypeService; }
        }

        /// <summary>
        /// Gets a dictionary containg the pre values associated with a property matching the
        /// specified <code>contentTypeAlias</code> and <code>propertyAlias</code>.
        /// </summary>
        /// <param name="contentTypeAlias">The alias of the content type.</param>
        /// <param name="propertyAlias">The alias of the property.</param>
        public static IDictionary<string, string> GetPreValues(string contentTypeAlias, string propertyAlias) {

            // Get a reference to the content type
            IContentType contentType = ContentTypeService.GetContentType(contentTypeAlias);
            if (contentType == null) return new Dictionary<string, string>();

            // Find the property matching the specified property alias
            PropertyType property = (
                contentType.PropertyTypes.FirstOrDefault(x => x.Alias == propertyAlias)
                ??
                contentType.CompositionPropertyTypes.FirstOrDefault(x => x.Alias == propertyAlias)
            );

            // Get the pre values based on the data type definition ID
            return property == null ? new Dictionary<string, string>() : GetPreValues(property.DataTypeDefinitionId);
        
        }

        /// <summary>
        /// Gets a dictionary containing the pre values of the data type definition with the specified <code>dataTypeDefinitionId</code>.
        /// </summary>
        /// <param name="dataTypeDefinitionId">The ID of the data type definition.</param>
        public static IDictionary<string, string> GetPreValues(int dataTypeDefinitionId) {
            PreValueCollection collection = DataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId);
            return collection == null ? new Dictionary<string, string>() : collection.PreValuesAsDictionary.ToDictionary(x => x.Key, x => x.Value.Value);
        }

    }

}
