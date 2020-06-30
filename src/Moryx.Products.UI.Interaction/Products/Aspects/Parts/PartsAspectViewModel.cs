// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Moryx.ClientFramework.Dialog;
using Moryx.Products.UI.Interaction.Properties;

namespace Moryx.Products.UI.Interaction.Aspects
{
    [ProductAspectRegistration(nameof(PartsAspectViewModel))]
    internal class PartsAspectViewModel : ProductAspectViewModelBase
    {
        public override string DisplayName => Strings.PartsAspectViewModel_DisplayName;

        #region Dependencies

        public IDialogManager DialogManager { get; set; }

        public IPartDialogsFactory PartDialogsFactory { get; set; }

        #endregion

        public IPartConnectorViewModel[] PartConnectors { get; private set; }

        private IPartConnectorViewModel _selectedPartConnector;
        public IPartConnectorViewModel SelectedPartConnector
        {
            get { return _selectedPartConnector; }
            set
            {
                var oldPartDetail = _selectedPartConnector;
                _selectedPartConnector = value;

                if (oldPartDetail != null)
                    ScreenExtensions.TryDeactivate(oldPartDetail, false);

                if (_selectedPartConnector != null)
                    ScreenExtensions.TryActivate(_selectedPartConnector);

                NotifyOfPropertyChange(nameof(SelectedPartConnector));
            }
        }

        /// <inheritdoc />
        public override bool IsRelevant(ProductViewModel product)
        {
            return product.Parts.Any();
        }

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            base.OnInitialize();

            var partConnectors = new List<IPartConnectorViewModel>(Product.Parts.Count);
            foreach (var partConnector in Product.Parts)
            {
                IPartConnectorViewModel partConnectorVm;
                if (partConnector.IsCollection)
                {
                    partConnectorVm = new CollectionPartConnectorViewModel(partConnector)
                    {
                        DialogManager = DialogManager,
                        PartDialogsFactory = PartDialogsFactory
                    };
                }
                else
                {
                    partConnectorVm = new SinglePartConnectorPartViewModel(partConnector)
                    {
                        DialogManager = DialogManager,
                        PartDialogsFactory = PartDialogsFactory
                    };
                }

                partConnectors.Add(partConnectorVm);
            }

            PartConnectors = partConnectors.ToArray();
            SelectedPartConnector = PartConnectors.FirstOrDefault();
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            foreach (var partConnector in PartConnectors)
                partConnector.BeginEdit();

            base.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            foreach (var partConnector in PartConnectors)
                partConnector.EndEdit();

            base.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            foreach (var partConnector in PartConnectors)
                partConnector.CancelEdit();

            base.CancelEdit();
        }
    }
}
