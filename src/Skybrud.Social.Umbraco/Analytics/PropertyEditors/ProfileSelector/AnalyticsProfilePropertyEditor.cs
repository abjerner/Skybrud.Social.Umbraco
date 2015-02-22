using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Social.Umbraco.Analytics.PropertyEditors.ProfileSelector {

    [PropertyEditor("Skybrud.Social.Analytics.ProfileSelector", "Skybrud.Social - Analytics Profile Selector", "/App_Plugins/Skybrud.Social/Analytics/ProfileSelector/PropertyEditor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Analytics/ProfileSelector/Controllers.js")]
    public class AnalyticsProfilePropertyEditor : PropertyEditor {
        
        protected override PreValueEditor CreatePreValueEditor() {
            return new AnalyticsProfilePreValueEditor();
        }

        [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/Skybrud.Social/Analytics/ProfileSelector/Controllers.js")]
        //[PropertyEditorAsset(ClientDependencyType.Css, "/App_Plugins/Skybrud.Social/Social.css")]
        internal class AnalyticsProfilePreValueEditor : PreValueEditor {

            [PreValueField("config", "Configuration", "/App_Plugins/Skybrud.Social/Analytics/ProfileSelector/PreValueEditor.html")]
			public string Config { get; set; }
        
        }
    
    }

}