using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Interface for a viewmodel with an edit mode
    /// </summary>
    public interface IEditModeViewModel : IScreen, IEditableObject
    {
        /// <summary>
        /// <c>true</c> if view model is in edit mode
        /// </summary>
        bool IsEditMode { get; }

        /// <summary>
        /// Indicates if the view is busy like during the save process
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// EditMode can be entered or not
        /// </summary>
        bool CanBeginEdit();

        /// <summary>
        /// EditMode can be canceled or not
        /// </summary>
        bool CanCancelEdit();

        /// <summary>
        /// Check if the current viewmodel can be saved
        /// </summary>
        bool CanEndEdit();

        void Validate(ICollection<ValidationResult> validationErrors);

        Task Save();
    }
}
