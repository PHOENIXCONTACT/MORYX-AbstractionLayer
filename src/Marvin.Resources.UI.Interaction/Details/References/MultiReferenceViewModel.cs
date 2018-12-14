using System.Collections.ObjectModel;
using System.Linq;
using C4I;
using Marvin.Resources.UI.Interaction.ResourceInteraction;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// View model for resource reference that have multiple targets
    /// </summary>
    public class MultiReferenceViewModel : ReferenceViewModel
    {
        /// <summary>
        /// Delegate to add targets to the reference
        /// </summary>
        public RelayCommand AddTargetsCmd { get; }

        /// <summary>
        /// Delegate to remove targets from the reference
        /// </summary>
        public RelayCommand RemoveTargetCmd { get; }

        /// <summary>
        /// Temporary collection used for the selected target references
        /// </summary>
        public ObservableCollection<ResourceViewModel> SelectedTargets { get; } =
            new ObservableCollection<ResourceViewModel>();

        internal MultiReferenceViewModel(ResourceReferenceModel model) : base(model)
        {
            AddTargetsCmd = new RelayCommand(AddToTargets, CanAddToTargets);
            RemoveTargetCmd = new RelayCommand(RemoveTarget, CanRemoveTarget);

            foreach (var target in Model.Targets)
                SelectedTargets.Add(new ResourceViewModel(target));
        }

        private bool CanAddToTargets(object parameters) =>
            parameters is ResourceViewModel && IsEditMode;

        private void AddToTargets(object parameters) => 
            SelectedTargets.Add((ResourceViewModel)parameters);

        private bool CanRemoveTarget(object parameters) =>
            parameters is ResourceViewModel && IsEditMode;

        private void RemoveTarget(object parameters) => 
            SelectedTargets.Remove((ResourceViewModel)parameters);

        /// <inheritdoc />
        public override void EndEdit()
        {
            base.EndEdit();

            Model.Targets.Clear();
            Model.Targets.AddRange(SelectedTargets.Select(t => t.Model));
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            base.CancelEdit();

            SelectedTargets.Clear();
            foreach (var target in Model.Targets)
                SelectedTargets.Add(new ResourceViewModel(target));
        }
    }
}