using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Skybrud.Social.Umbraco {

    public static class PreValueHelpers {

        internal static IContentTypeService ContentTypeService {
            get { return UmbracoContext.Current.Application.Services.ContentTypeService; }
        }

        internal static IDataTypeService DataTypeService {
            get { return UmbracoContext.Current.Application.Services.DataTypeService; }
        }

        public static IDictionary<string, string> GetPreValues(string contentTypeAlias, string propertyAlias) {
            IContentType contentType = ContentTypeService.GetContentType(contentTypeAlias);
            PropertyType property = contentType == null ? null : contentType.PropertyTypes.FirstOrDefault(x => x.Alias == propertyAlias);
            return property == null ? new Dictionary<string, string>() : GetPreValues(property.DataTypeDefinitionId);
        }

        public static IDictionary<string, string> GetPreValues(int dataTypeDefinitionId) {
            PreValueCollection collection = DataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId);
            return collection == null ? new Dictionary<string, string>() : collection.PreValuesAsDictionary.ToDictionary(x => x.Key, x => x.Value.Value);
        }

    }

}
