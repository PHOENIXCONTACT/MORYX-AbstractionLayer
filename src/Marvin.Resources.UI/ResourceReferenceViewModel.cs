using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Marvin.Resources.UI.ResourceService;
using Marvin.Tools;

namespace Marvin.Resources.UI
{

    /// <summary>
    /// View model representing a reference property on a resource
    /// </summary>
    public class ResourceReferenceViewModel : PropertyChangedBase, IEditableObject
    {
        #region Fields and Properties

        /// <summary>
        /// Model of this resource reference
        /// </summary>
        public ResourceReferenceModel Model { get; private set; }

        /// <summary>
        /// Additional type information for the reference
        /// </summary>
        public ReferenceTypeViewModel Type { get; private set; }

        /// <summary>
        /// Display name of this reference
        /// </summary>
        public string DisplayName => Type.DisplayName;

        /// <summary>
        /// Description of this reference
        /// </summary>
        public string Description => Type.Description;

        /// <summary>
        /// Indicator if this reference is a collection
        /// </summary>
        public bool IsCollection => Type.IsCollection;

        /// <summary>
        /// Currently selected targets
        /// </summary>
        public ObservableCollection<IResourceViewModel> Targets { get; } = new ObservableCollection<IResourceViewModel>();

        /// <summary>
        /// Returns the info if there is any target referenced
        /// </summary>
        public bool AnyReference => Targets.Any();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceReferenceViewModel"/> class.
        /// </summary>
        public ResourceReferenceViewModel(ResourceReferenceModel model, ReferenceTypeViewModel type)
        {
            Model = model;
            CopyFromModel();

            Type = type;
            Targets.CollectionChanged += (sender, args) => NotifyOfPropertyChange(nameof(AnyReference));
        }

        /// <inheritdoc />
        public void BeginEdit()
        {
        }

        /// <inheritdoc />
        public void EndEdit()
        {
            CopyToModel();
        }

        /// <inheritdoc />
        public void CancelEdit()
        {
            CopyFromModel();
        }

        private void CopyFromModel()
        {
            var targets = Model.Targets.Select(model => new ResourceInfoViewModel(model));
            Targets.Clear();
            Targets.AddRange(targets);
        }

        private void CopyToModel()
        {
            Model.Targets = Targets.Select(t => t.Model).ToArray();
        }

        public void UpdateModel(ResourceReferenceModel model, ReferenceTypeViewModel referenceType)
        {
            Model = model;
            Type = referenceType;
            CopyFromModel();
        }
    }
}