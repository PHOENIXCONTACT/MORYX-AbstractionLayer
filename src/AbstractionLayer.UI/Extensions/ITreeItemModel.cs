using System.Collections.Generic;

namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Common API for TreeItem Models and TreeItemViewModels
    /// </summary>
    public interface ITreeItemModel<out TModel> : IIdentifiableObject
    {
        /// <summary>
        /// Recursive children of the branch
        /// </summary>
        IReadOnlyList<TModel> Children { get; }
    }
}
