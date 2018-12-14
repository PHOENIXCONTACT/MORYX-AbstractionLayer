using System.Collections.Generic;
using System.ComponentModel;
using Caliburn.Micro;
using Marvin.Resources.UI.Interaction.ResourceInteraction;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// View model representing a reference property on a resource
    /// </summary>
    public abstract class ReferenceViewModel : Screen, IEditableObject
    {
        private bool _isEditMode;

        #region Fields and Properties

        internal ResourceReferenceModel Model { get; }

        /// <summary>
        /// Name of the reference
        /// </summary>
        public string Name => Model.Name;

        /// <summary>
        /// Description of this reference
        /// </summary>
        public string Description => Model.Description;

        /// <summary>
        /// This reference is a collection
        /// </summary>
        public bool IsCollection => Model.IsCollection;

        /// <summary>
        /// List of references the user can choose from to use for this reference
        /// </summary>
        public ICollection<ResourceViewModel> PossibleTargets { get; } = new List<ResourceViewModel>();

        /// <summary>
        /// Indicator if view model is currently in edit mode
        /// </summary>
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (value == _isEditMode)
                    return;
                _isEditMode = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceViewModel"/> class.
        /// </summary>
        internal ReferenceViewModel(ResourceReferenceModel reference)
        {
            Model = reference;

            foreach (var target in Model.PossibleTargets)
                PossibleTargets.Add(new ResourceViewModel(target));
        }

        /// <inheritdoc />
        public virtual void BeginEdit() =>
            IsEditMode = true;

        /// <inheritdoc />
        public virtual void EndEdit() =>
            IsEditMode = false;

        /// <inheritdoc />
        public virtual void CancelEdit() =>
            IsEditMode = false;

        /// <summary>
        /// Create a reference view model
        /// </summary>
        internal static ReferenceViewModel Create(ResourceReferenceModel reference)
        {
            return reference.IsCollection
                ? (ReferenceViewModel)new MultiReferenceViewModel(reference)
                : new SingleReferenceViewModel(reference);
        }
    }
}