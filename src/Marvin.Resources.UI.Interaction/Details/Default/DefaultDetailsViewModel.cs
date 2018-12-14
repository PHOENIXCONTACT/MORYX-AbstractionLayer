using System.Linq;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.Controls;

namespace Marvin.Resources.UI.Interaction
{
    [ResourceDetailsRegistration(DetailsConstants.DefaultType)]
    internal class DefaultDetailsViewModel : ResourceDetailsViewModelBase
    {
        private EntryViewModel _configViewModel;

        public EntryViewModel ConfigViewModel
        {
            get { return _configViewModel; }
            set
            {
                _configViewModel = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        protected override Task OnConfigLoaded()
        {
            ConfigViewModel = new EntryViewModel(ConfigEntries);

            return base.OnConfigLoaded();
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            ConfigViewModel = new EntryViewModel(ConfigEntries.Clone(true));
            base.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            ConfigEntries = ConfigViewModel.Entry;
            base.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            ConfigViewModel = new EntryViewModel(ConfigEntries);
            base.CancelEdit();
        }
    }
}