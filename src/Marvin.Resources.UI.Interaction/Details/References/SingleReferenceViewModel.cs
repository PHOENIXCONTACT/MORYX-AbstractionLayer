using System.Linq;
using C4I;
using Marvin.Resources.UI.Interaction.ResourceInteraction;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// View model for resource references that only reference a single target
    /// </summary>
    public class SingleReferenceViewModel : ReferenceViewModel
    {
        /// <summary>
        /// Command to set the selected reference
        /// </summary>
        public RelayCommand SetTargetCmd { get; }

        /// <summary>
        /// Command to clear the current target
        /// </summary>
        public RelayCommand ClearTargetCmd { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        internal SingleReferenceViewModel(ResourceReferenceModel model) : base(model)
        {
            SetTargetCmd = new RelayCommand(SetTarget, CanSelectTarget);
            ClearTargetCmd = new RelayCommand(ClearTarget, CanClearTarget);

            if (model.Targets.Count == 1)
                SelectedTarget = PossibleTargets.SingleOrDefault(p => p.Id == Model.Targets[0].Id);
        }

        private bool CanSelectTarget(object obj) =>
            obj is ResourceViewModel && IsEditMode;

        private void SetTarget(object parameters) =>
            SelectedTarget = (ResourceViewModel) parameters;

        private bool CanClearTarget(object parameters) =>
            SelectedTarget != null && IsEditMode;

        private void ClearTarget(object parameters) =>
            SelectedTarget = null;

        private ResourceViewModel _selectedTarget;

        /// <summary>
        /// Currently selected Driver from the PossibleDrivers list
        /// </summary>
        public ResourceViewModel SelectedTarget
        {
            get { return _selectedTarget; }
            set
            {
                if (_selectedTarget == value)
                    return;
                _selectedTarget = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            base.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            base.EndEdit();

            if (SelectedTarget == null)
                Model.Targets.Clear();
            else if (Model.Targets.Count == 0)
                Model.Targets.Add(SelectedTarget.Model);
            else
                Model.Targets[0] = SelectedTarget.Model;
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            base.CancelEdit();

            SelectedTarget = Model.Targets.Count == 1
                ? PossibleTargets.SingleOrDefault(p => p.Id == Model.Targets[0].Id)
                : null;
        }
    }
}