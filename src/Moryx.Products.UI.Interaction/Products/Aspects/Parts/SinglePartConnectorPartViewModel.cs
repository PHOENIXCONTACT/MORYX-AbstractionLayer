// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Moryx.WpfToolkit;
using Moryx.AbstractionLayer.UI;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;

namespace Moryx.Products.UI.Interaction.Aspects
{
    internal class SinglePartConnectorPartViewModel : EditModeViewModelBase, IPartConnectorViewModel
    {
        private PartLinkViewModel _partLink;

        #region Dependencies

        public IPartDialogsFactory PartDialogsFactory { get; set; }

        public IDialogManager DialogManager { get; set; }

        #endregion

        public PartConnectorViewModel PartConnector { get; }

        public PartLinkViewModel PartLink
        {
            get => _partLink;
            private set
            {
                _partLink = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Command to change the part link
        /// </summary>
        public ICommand ChangeLinkCmd { get; }

        /// <summary>
        /// Command to clear the part link
        /// </summary>
        public ICommand ClearLinkCmd { get; }

        /// <summary>
        /// Constructor for single usage
        /// </summary>
        public SinglePartConnectorPartViewModel(PartConnectorViewModel partConnector)
        {
            PartConnector = partConnector;
            ChangeLinkCmd = new AsyncCommand(ChangeLink, CanChangeLink, true);
            ClearLinkCmd = new RelayCommand(ClearLink, CanClearLink);

            PartLink = PartConnector.PartLinks.FirstOrDefault();
        }
        /// <summary>
        /// Constructor for usage in collections
        /// </summary>
        public SinglePartConnectorPartViewModel(PartConnectorViewModel partConnector, PartLinkViewModel partLink)
        {
            PartConnector = partConnector;
            ChangeLinkCmd = new AsyncCommand(ChangeLink, CanChangeLink, true);
            ClearLinkCmd = new RelayCommand(ClearLink, CanClearLink);

            PartLink = partLink;
        }

        public override void CancelEdit()
        {
            // If single used, reset part link
            if (!PartConnector.IsCollection)
                PartLink = PartConnector.PartLinks.FirstOrDefault();

            base.CancelEdit();
        }

        private bool CanClearLink(object obj) =>
            IsEditMode && !PartConnector.IsCollection;

        private void ClearLink(object obj)
        {
            PartConnector.PartLinks.Clear();
            PartLink = null;
        }

        private bool CanChangeLink(object obj) =>
            IsEditMode && !PartConnector.IsCollection;

        private async Task ChangeLink(object obj)
        {
            var dialog = PartDialogsFactory.CreateSelectPartDialog(PartConnector);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
            {
                PartDialogsFactory.Destroy(dialog);
                return;
            }

            var partLink = PartConnector.CreatePartLink(dialog.SelectedProduct);

            if (!PartConnector.IsCollection)
                PartConnector.PartLinks.Clear();

            PartConnector.PartLinks.Add(partLink);
            PartLink = partLink;
        }
    }
}
