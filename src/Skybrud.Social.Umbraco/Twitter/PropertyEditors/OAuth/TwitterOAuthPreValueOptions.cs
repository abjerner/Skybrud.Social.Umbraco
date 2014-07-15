using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybrud.Social.Umbraco.Twitter.PropertyEditors.OAuth {

    public class TwitterOAuthPreValueOptions {

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public bool IsValid {
            get {
                if (String.IsNullOrEmpty(ConsumerKey)) return false;
                if (String.IsNullOrEmpty(ConsumerSecret)) return false;
                return true;
            }
        }

        public static TwitterOAuthPreValueOptions Get(string contentTypeAlias, string propertyAlias) {

            IDictionary<string, string> prevalues = PreValueHelpers.GetPreValues(contentTypeAlias, propertyAlias);

            string config;
            prevalues.TryGetValue("config", out config);

            try {
                return JsonConvert.DeserializeObject<TwitterOAuthPreValueOptions>(config);
            } catch (Exception) {
                return new TwitterOAuthPreValueOptions();
            }

        }

    }

}
