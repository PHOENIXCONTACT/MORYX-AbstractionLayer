// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

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
