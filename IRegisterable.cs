using Nanoray.PluginManager;
using Nickel;

namespace Weth;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}