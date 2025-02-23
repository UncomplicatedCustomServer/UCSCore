using UncomplicatedCustomServerCore.API.Features.Bans;
using UncomplicatedCustomServerCore.API.Features.Warns;

namespace UncomplicatedCustomServerCore.API.Features
{
    internal class FeaturesManager
    {
        public static async void Initialize()
        {
            await Warn.Syncronize();
            await Ban.Syncronize();
        }
    }
}
