// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using C4I;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Resources.UI.Interaction.Properties;
using Moryx.Resources.UI.ResourceService;
using Moryx.Tools;

namespace Moryx.Resources.UI.Interaction
{
    internal class TypeSelectorViewModel : DialogScreen
    {
        private readonly IResourceServiceModel _resourceServiceModel;
        private ResourceTypeViewModel _selectedType;
        private string _errorMessage;
        private TaskNotifier _taskNotifier;

        #region Fields and properties

        public ICommand CancelCmd { get; }

        public AsyncCommand CreateCmd { get; }

        /// <summary>
        /// Type tree is set depending on selected node
        /// </summary>
        public IEnumerable<ResourceTypeViewModel> TypeTree { get; set; }

        public ResourceTypeViewModel SelectedType
        {
            get { return _selectedType; }
            private set
            {
                _selectedType = value;
                NotifyOfPropertyChange();
                CreateCmd.RaiseCanExecuteChanged();
            }
        }

        public ResourceModel ResourcePrototype { get; private set; }

        /// <summary>
        /// Error message while adding the resource
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public TypeSelectorViewModel(IResourceServiceModel resourceServiceModel, ResourceTreeItemViewModel resource)
        {
            _resourceServiceModel = resourceServiceModel;

            var rootType = _resourceServiceModel.TypeTree;
            var constraint = new[] { rootType.Name };
            // Extract children constraint
            if (resource != null)
            {
                var resourceType = _resourceServiceModel.TypeTree.DerivedTypes
                    .Flatten(t => t.DerivedTypes).First(t => t.Name == resource.Resource.TypeName);
                constraint = resourceType.References.First(r => r.Name == "Children").SupportedTypes;
            }

            var matches = FilterTypes(rootType, constraint);
            // If there is a single, abstract root node, we can skip it
            if (matches.Count == 1 && !matches[0].Creatable)
                matches = matches[0].DerivedTypes;

            TypeTree = matches.Select(type => new ResourceTypeViewModel(type));

            CreateCmd = new AsyncCommand(Create, CanCreate);
            CancelCmd = new RelayCommand(Cancel, CanCancel);
        }

        private static IReadOnlyList<ResourceTypeModel> FilterTypes(ResourceTypeModel typeNode, string[] constraint)
        {
            if (constraint.Contains(typeNode.Name))
                return new[] { typeNode };

            return typeNode.DerivedTypes.SelectMany(dt => FilterTypes(dt, constraint)).ToArray();
        }

        ///
        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = Strings.TypeSelectorViewModel_DisplayName;
        }

        /// <summary>
        /// Checks if the resource type can be created <see cref="CreateCmd"/>
        /// </summary>
        private bool CanCreate(object obj) =>
            SelectedType != null && SelectedType.Creatable && _resourceServiceModel.IsAvailable;

        /// <summary>
        /// Will be called by <see cref="CreateCmd"/> and will return a true result
        /// </summary>
        private async Task Create(object obj)
        {
            try
            {
                ResourcePrototype = await _resourceServiceModel.CreateResource(SelectedType.Name, SelectedType.Constructors.FirstOrDefault(c => c.IsSelected)?.Model);
                TryClose(true);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        private bool CanCancel(object obj) =>
            !CreateCmd.IsExecuting;

        /// <summary>
        /// Will be called by <see cref="CancelCmd"/> and will return a false result
        /// </summary>
        private void Cancel(object obj) =>
            TryClose(false);

        public void OnTreeItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            var selected = (ResourceTypeViewModel)args.NewValue;
            SelectedType = selected;
        }
    }
}
