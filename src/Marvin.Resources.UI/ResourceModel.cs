using System.Collections.Generic;
using System.Linq;
using Marvin.AbstractionLayer.UI;

// ReSharper disable once CheckNamespace
namespace Marvin.Resources.UI.ResourceService
{
    public partial class ResourceModel : ITreeItemModel<ResourceModel>
    {
        public IReadOnlyList<ResourceModel> Children => References
            .First(r => r.Name == "Children").Targets;
    }
}