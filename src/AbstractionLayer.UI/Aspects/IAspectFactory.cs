using Marvin.Container;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    [PluginFactory(typeof(INameBasedComponentSelector))]
    public interface IAspectFactory
    {
        IAspect Create(string name);

        void Destroy(IAspect aspect);
    }
}
