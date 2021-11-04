// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace Moryx.AbstractionLayer.UI
{
    /// <summary>
    /// Base class for ViewModels with an edit mode
    /// </summary>
    public abstract class EditModeViewModelBase : Screen, IEditModeViewModel
    {
        #region Fields and Properties

        private bool _isEditMode;
        private bool _isBusy;

        /// <summary>
        /// Static representation of a successful task
        /// </summary>
        protected static Task<bool> SuccessTask = Task.FromResult(true);

        #endregion

        /// <inheritdoc />
        public bool IsEditMode
        {
            get => _isEditMode;
            private set
            {
                _isEditMode = value;
                NotifyOfPropertyChange(nameof(IsEditMode));
            }
        }

        /// <inheritdoc />
        public bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                _isBusy = value;
                NotifyOfPropertyChange(nameof(IsBusy));
            }
        }

        /// <inheritdoc />
        public virtual bool CanBeginEdit()
        {
            return IsEditMode == false;
        }

        /// <inheritdoc />
        public virtual bool CanCancelEdit()
        {
            return !CanBeginEdit();
        }

        /// <inheritdoc />
        public virtual bool CanEndEdit()
        {
            return IsEditMode;
        }

        /// <inheritdoc />
        public virtual void Validate(ICollection<ValidationResult> validationErrors)
        {
        }

        /// <inheritdoc />
        public virtual Task Save()
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public virtual void BeginEdit()
        {
            IsEditMode = true;
        }

        /// <inheritdoc />
        public virtual void EndEdit()
        {
            IsEditMode = false;
        }

        /// <inheritdoc />
        public virtual void CancelEdit()
        {
            IsEditMode = false;
        }
    }

    /// <summary>
    /// Base class for ViewModels with an edit mode
    /// </summary>
    public abstract class EditModeViewModelBase<T> : EditModeViewModelBase
        where T : IEditableObject
    {
        private T _editableObject;

        /// <summary>
        /// Represents the editable object
        /// </summary>
        public T EditableObject
        {
            get { return _editableObject; }
            protected set
            {
                _editableObject = value;
                NotifyOfPropertyChange(nameof(EditableObject));
            }
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            base.BeginEdit();
            EditableObject.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            EditableObject.EndEdit();
            base.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            EditableObject.CancelEdit();
            base.CancelEdit();
        }
    }
}
