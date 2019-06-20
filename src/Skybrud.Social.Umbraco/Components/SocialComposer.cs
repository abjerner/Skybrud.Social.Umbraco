using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Skybrud.Social.Umbraco.Components {

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SocialComposer : IUserComposer {

        public void Compose(Composition composition) {
            composition.Components().Append<SocialComponent>();
        }

    }

}