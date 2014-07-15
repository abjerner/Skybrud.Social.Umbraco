using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth {

    [PropertyEditor("Skybrud.Social.Facebook.OAuth", "Skybrud.Social - Facebook OAuth", "/App_Plugins/Skybrud.Social/Facebook/OAuth/PropertyEditor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Facebook/OAuth/Controllers.js")]
    public class FacebookOAuthPropertyEditor : PropertyEditor {
        
        protected override PreValueEditor CreatePreValueEditor() {
            return new FacebookOAuthPreValueEditor();
        }

        [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Facebook/OAuth/Controllers.js")]
        //[PropertyEditorAsset(ClientDependencyType.Css, "/App_Plugins/Skybrud.Social/Social.css")]
        internal class FacebookOAuthPreValueEditor : PreValueEditor {
            
            [PreValueField("config", "Configuration", "/App_Plugins/Skybrud.Social/Facebook/OAuth/PreValueEditor.html")]
			public string Config { get; set; }
        
        }
    
    }

}