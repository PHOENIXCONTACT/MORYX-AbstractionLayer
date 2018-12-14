using System.Collections.Generic;
using System.ComponentModel;
using Caliburn.Micro;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// View model used to display the resource references
    /// </summary>
    public class ReferenceCollectionViewModel : Conductor<ReferenceViewModel>.Collection.OneActive, IEditableObject
    {
        private ReferenceViewModel _selected;
        private bool _isEditMode;

        /// <summary>
        /// Currently selected reference
        /// </summary>
        public ReferenceViewModel Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value)
                    return;
                _selected = value; 
                ActivateItem(value);
            }
        }

        /// <summary>
        /// Inidicator if the collection is currently in edit mode
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

        /// <summary>
        /// Constructor to create the collection view model
        /// </summary>
        public ReferenceCollectionViewModel(IEnumerable<ReferenceViewModel> references)
        {
            Items.AddRange(references);
        }

        /// <inheritdoc />
        public void BeginEdit()
        {
            foreach (var reference in Items)
                reference.BeginEdit();

            IsEditMode = true;
        }

        /// <inheritdoc />
        public void EndEdit()
        {
            foreach (var reference in Items)
                reference.EndEdit();

            IsEditMode = false;
        }

        /// <inheritdoc />
        public void CancelEdit()
        {
            foreach (var reference in Items)
                reference.CancelEdit();
            IsEditMode = false;
        }
    }
}