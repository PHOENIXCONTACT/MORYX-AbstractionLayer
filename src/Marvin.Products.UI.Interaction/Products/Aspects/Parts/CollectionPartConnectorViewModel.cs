// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C4I;
using Marvin.AbstractionLayer.UI;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.Tools;

namespace Marvin.Products.UI.Interaction.Aspects
{
    internal class CollectionPartConnectorViewModel : EditModeViewModelBase, IPartConnectorViewModel
    {
        #region Dependencies

        public IPartDialogsFactory PartDialogsFactory { get; set; }

        public IDialogManager DialogManager { get; set; }

        #endregion

        private readonly ICollection<SinglePartConnectorPartViewModel> _removedLinks = new List<SinglePartConnectorPartViewModel>();
        private readonly ICollection<SinglePartConnectorPartViewModel> _newLinks = new List<SinglePartConnectorPartViewModel>();

        public PartConnectorViewModel PartConnector { get; }

        public ICommand AddCmd { get; }

        public ICommand RemoveCmd { get; }

        public ObservableCollection<SinglePartConnectorPartViewModel> PartLinks { get; } =
            new ObservableCollection<SinglePartConnectorPartViewModel>();

        private SinglePartConnectorPartViewModel _selectedPartLink;
        public SinglePartConnectorPartViewModel SelectedPartLink
        {
            get { return _selectedPartLink; }
            set
            {
                _selectedPartLink = value;
                NotifyOfPropertyChange();
            }
        }

        public CollectionPartConnectorViewModel(PartConnectorViewModel partConnector)
        {
            PartConnector = partConnector;

            AddCmd = new AsyncCommand(OnAdd, CanAdd, true);
            RemoveCmd = new RelayCommand(Remove, CanRemove);

            PartConnector.PartLinks.ForEach(pl => AddPartLink(partConnector, pl));
            SelectedPartLink = PartLinks.FirstOrDefault();
        }

        private SinglePartConnectorPartViewModel AddPartLink(PartConnectorViewModel partConnector, PartLinkViewModel partLink)
        {
            var link = new SinglePartConnectorPartViewModel(partConnector, partLink)
            {
                DialogManager = DialogManager,
                PartDialogsFactory = PartDialogsFactory
            };
            PartLinks.Add(link);

            return link;
        }

        public override void BeginEdit()
        {
            base.BeginEdit();

            PartLinks.ForEach(p => p.BeginEdit());
        }

        public override void EndEdit()
        {
            // End on part links
            PartLinks.ForEach(p => p.EndEdit());

            // Add new links
            var newLinks = _newLinks.Select(n => n.PartLink);
            PartConnector.PartLinks.AddRange(newLinks);
            _newLinks.Clear();

            // Remove removed links from connector
            PartConnector.PartLinks.RemoveRange(_removedLinks.Select(l => l.PartLink));
            _removedLinks.Clear();

            base.EndEdit();
        }

        public override void CancelEdit()
        {
            // Remove newly created part links
            PartLinks.RemoveRange(_newLinks);
            _newLinks.Clear();

            // Add removed again
            PartLinks.AddRange(_removedLinks);
            _removedLinks.Clear();

            // Cancel on existent
            PartLinks.ForEach(p => p.CancelEdit());

            base.CancelEdit();
        }

        private bool CanAdd(object obj) =>
            IsEditMode;

        private async Task OnAdd(object obj)
        {
            var dialog = PartDialogsFactory.CreateSelectPartDialog(PartConnector);
            await DialogManager.ShowDialogAsync(dialog);
            if (!dialog.Result)
            {
                PartDialogsFactory.Destroy(dialog);
                return;
            }

            var newLink = AddPartLink(PartConnector, dialog.PartLink);
            newLink.BeginEdit();

            _newLinks.Add(newLink);
            SelectedPartLink = newLink;

            PartDialogsFactory.Destroy(dialog);
        }

        private bool CanRemove(object obj) =>
            IsEditMode && SelectedPartLink != null;


        private void Remove(object obj)
        {
            var idx = PartLinks.IndexOf(SelectedPartLink);

            // If is not new, add to list of removed part links
            if (!_newLinks.Contains(SelectedPartLink))
                _removedLinks.Add(SelectedPartLink);
            else
                _newLinks.Remove(SelectedPartLink);

            PartLinks.Remove(SelectedPartLink);

            if (idx >= PartLinks.Count)
                idx = PartLinks.Count - 1;

            SelectedPartLink = idx >= 0 ? PartLinks[idx] : null;
        }
    }
}
