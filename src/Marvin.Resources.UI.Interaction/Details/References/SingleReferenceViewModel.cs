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

        internal SingleReferenceViewModel(ResourceReferenceModel model) : base(model)
        {
            SetTargetCmd = new RelayCommand(SetTarget, CanSelectTarget);
            SelectedTarget = PossibleTargets.SingleOrDefault(p => p.Name == Model.Targets[0].Name);
        }

        private bool CanSelectTarget(object obj) => 
            obj is ResourceViewModel && IsEditMode;

        private void SetTarget(object parameters) => 
            SelectedTarget = (ResourceViewModel) parameters;

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

            SelectedTarget = PossibleTargets.SingleOrDefault(p => p.Name == Model.Targets[0].Name);
        }
    }
}