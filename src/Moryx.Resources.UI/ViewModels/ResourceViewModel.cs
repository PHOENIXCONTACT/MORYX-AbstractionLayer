// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Resources.UI.ResourceService;
using Moryx.Tools;
using Entry = Moryx.Serialization.Entry;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// Resource view model for the <see cref="ResourceModel"/>
    /// </summary>
    public class ResourceViewModel : PropertyChangedBase, IEditableObject, IResourceViewModel
    {
        private string _name;
        private string _description;
        private EntryViewModel _properties;
        private ResourceMethodViewModel[] _methods;

        /// <summary>
        /// Underlying model of this view model
        /// </summary>
        public ResourceModel Model { get; private set; }

        /// <summary>
        /// Type model for this instance
        /// </summary>
        public ResourceTypeViewModel Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceViewModel"/> class.
        /// </summary>
        public ResourceViewModel(ResourceModel model, ResourceTypeViewModel type)
        {
            Type = type;
            UpdateModel(model);
        }

        /// <summary>
        /// Id of the resource
        /// </summary>
        public long Id => Model.Id;

        /// <summary>
        /// Name of the resource
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Typename of the resource
        /// </summary>
        public string TypeName => Model.Type;

        /// <summary>
        /// Description of this resource
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyOfPropertyChange();
            }
        }

        // TODO: AL 6 Remove public setter
        /// <summary>
        /// Properties of this resource
        /// </summary>
        public EntryViewModel Properties
        {
            get => _properties;
            set
            {
                if (_properties is null || value is null)
                {
                    _properties = value;
                    NotifyOfPropertyChange();
                }
                else
                    _properties.UpdateModel(value.Entry);
            }
        }

        /// <summary>
        /// Methods which can be called by this resource
        /// </summary>
        public ResourceMethodViewModel[] Methods
        {
            get => _methods;
            set
            {
                _methods = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// References to other resources
        /// </summary>
        public ObservableCollection<ResourceReferenceViewModel> References { get; } = new ObservableCollection<ResourceReferenceViewModel>();

        #region IEditableObject


        /// <inheritdoc />
        public void BeginEdit()
        {
            References.BeginEdit();
            Properties.BeginEdit();
            Methods.BeginEdit();
        }

        /// <inheritdoc />
        public void EndEdit()
        {
            Properties.EndEdit();
            References.EndEdit();
            Methods.EndEdit();
            CopyToModel();
        }

        /// <inheritdoc />
        public void CancelEdit()
        {
            Properties.CancelEdit();
            References.CancelEdit();
            Methods.CancelEdit();
            CopyFromModel();
        }

        /// <summary>
        /// Updates the internal model and raises NotifyPropertyChanged
        /// </summary>
        /// <param name="resourceModel">The updated model instance</param>
        public void UpdateModel(ResourceModel resourceModel)
        {
            Model = resourceModel;
            CopyFromModel();
        }

        /// <summary>
        /// Copies view model data to the underlying DTO
        /// </summary>
        private void CopyToModel()
        {
            Model.Name = Name;
            Model.Description = Description;
            Model.Properties = Properties.Entry.ToServiceEntry();
            Model.References = References.Select(r => r.Model).ToArray();
        }

        /// <summary>
        /// Copies data of the underlying DTO the the view model
        /// </summary>
        private void CopyFromModel()
        {
            Name = Model.Name;
            Description = Model.Description;

            Properties = Model.Properties != null
                ? new EntryViewModel(Model.Properties.ToSerializationEntry())
                : new EntryViewModel();

            Methods = Model.Methods != null
                ? Model.Methods.Select(m => new ResourceMethodViewModel(m)).ToArray()
                : new ResourceMethodViewModel[0];

            if (Model.References != null)
            {
                var referenceModels = Model.References.ToArray();
                foreach (var referenceModel in referenceModels)
                {
                    var referenceType = Type.References.First(type => type.Model.Name == referenceModel.Name);
                    var existing = References.FirstOrDefault(r => r.Model.Name == referenceModel.Name);
                    if (existing != null)
                        existing.UpdateModel(referenceModel, referenceType);
                    else
                    {
                        var referenceVm = new ResourceReferenceViewModel(referenceModel, referenceType);
                        References.Add(referenceVm);
                    }
                }

                References.RemoveBy(rm => !referenceModels.Select(m => m.Name).Contains(rm.Model.Name));
            }
        }

        #endregion
    }
}
