// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace Moryx.AbstractionLayer.UI
{
    /// <summary>
    /// Generic tree item implementation
    /// </summary>
    public abstract class TreeItemViewModel : PropertyChangedBase, IIdentifiableObject
    {
        private bool _isExpanded;
        private bool _isSelected;

        /// <inheritdoc />
        public abstract long Id { get; }

        /// <summary>
        /// Children of this tree item
        /// </summary>
        public ObservableCollection<TreeItemViewModel> Children { get; } = new();

        /// <summary>
        /// Display name of this TreeViewItem
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
