using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Social.Umbraco.Google.PropertyEditors.OAuth {

    [PropertyEditor("Skybrud.Social.Google.OAuth", "Skybrud.Social - Google OAuth", "/App_Plugins/Skybrud.Social/Google/OAuth/PropertyEditor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Google/OAuth/Controllers.js")]
    public class GoogleOAuthPropertyEditor : PropertyEditor {
        
        protected override PreValueEditor CreatePreValueEditor() {
            return new GoogleOAuthPreValueEditor();
        }

        [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Google/OAuth/Controllers.js")]
        //[PropertyEditorAsset(ClientDependencyType.Css, "/App_Plugins/Skybrud.Social/Social.css")]
        internal class GoogleOAuthPreValueEditor : PreValueEditor {
            
            [PreValueField("config", "Configuration", "/App_Plugins/Skybrud.Social/Google/OAuth/PreValueEditor.html")]
			public string Config { get; set; }
        
        }
    
    }

}