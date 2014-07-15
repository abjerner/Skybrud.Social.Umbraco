using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Social.Umbraco.Twitter.PropertyEditors.OAuth {

    [PropertyEditor("Skybrud.Social.Twitter.OAuth", "Skybrud.Social - Twitter OAuth", "/App_Plugins/Skybrud.Social/Twitter/OAuth/PropertyEditor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Twitter/OAuth/Controllers.js")]
    public class TwitterOAuthPropertyEditor : PropertyEditor {
        
        protected override PreValueEditor CreatePreValueEditor() {
            return new TwitterPreValueEditor();
        }

        [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Twitter/OAuth/Controllers.js")]
        internal class TwitterPreValueEditor : PreValueEditor {

            [PreValueField("config", "Configuration", "/App_Plugins/Skybrud.Social/Twitter/OAuth/PreValueEditor.html")]
			public string Config { get; set; }
        
        }
    
    }

}