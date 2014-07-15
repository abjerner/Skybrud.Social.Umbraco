using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Social.Umbraco.Instagram.PropertyEditors.OAuth {

    [PropertyEditor("Skybrud.Social.Instagram.OAuth", "Skybrud.Social - Instagram OAuth", "/App_Plugins/Skybrud.Social/Instagram/OAuth/PropertyEditor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Instagram/OAuth/Controllers.js")]
    public class InstagramOAuthPropertyEditor : PropertyEditor {
        
        protected override PreValueEditor CreatePreValueEditor() {
            return new InstagramOAuthPreValueEditor();
        }

        [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Instagram/OAuth/Controllers.js")]
        //[PropertyEditorAsset(ClientDependencyType.Css, "/App_Plugins/Skybrud.Social/Social.css")]
        internal class InstagramOAuthPreValueEditor : PreValueEditor {
            
            [PreValueField("config", "Configuration", "/App_Plugins/Skybrud.Social/Instagram/OAuth/PreValueEditor.html")]
			public string Config { get; set; }
        
        }
    
    }

}