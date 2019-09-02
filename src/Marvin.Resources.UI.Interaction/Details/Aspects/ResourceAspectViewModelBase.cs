using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;

namespace Marvin.Resources.UI.Interaction.Aspects
{
    /// <summary>
    /// Abstract base class for product aspects
    /// </summary>
    public abstract class ResourceAspectViewModelBase : EditModeViewModelBase, IResourceAspect
    {
        /// <summary>
        /// <see cref="ResourceViewModel"/> instance of the aspect
        /// </summary>
        public ResourceViewModel Resource { get; set; }

        /// <inheritdoc />
        public virtual bool IsRelevant(ResourceViewModel resource) => true;

        /// <inheritdoc />
        public virtual Task Load(ResourceViewModel resource)
        {
            Resource = resource;
            return Task.FromResult(true);
        }

        /// <inheritdoc cref="IResourceAspect" />
        public override Task Save() => Task.FromResult(true);
    }
}
