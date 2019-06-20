using System.Collections.Generic;
using Skybrud.Social.Umbraco.Constants;
using Umbraco.Core.Composing;
using Umbraco.Web.JavaScript;

namespace Skybrud.Social.Umbraco.Components {
    public class SocialComponent : IComponent {

        public void Initialize() {
            ServerVariablesParser.Parsing += ServerVariablesParserOnParsing;
        }

        public void Terminate() { }

        private void ServerVariablesParserOnParsing(object sender, Dictionary<string, object> dictionary) {
            dictionary["skybrudSocial"] = new Dictionary<string, object> {
                { "webApiBaseUrl", $"/umbraco/backoffice/{SocialConstants.PluginController}/" },
                { "surfaceBaseUrl", $"/umbraco/{SocialConstants.PluginController}/" }
            };
        }

    }

}