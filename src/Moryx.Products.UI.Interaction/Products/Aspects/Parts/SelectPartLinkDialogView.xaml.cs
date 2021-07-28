// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Action = System.Action;

namespace Moryx.Products.UI.Interaction.Aspects
{
    /// <summary>
    /// Interaction logic for SelectPartLinkDialogView.xaml
    /// </summary>
    public partial class SelectPartLinkDialogView
    {
        private readonly CollectionViewSource _viewSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectPartLinkDialogView"/> class.
        /// </summary>
        public SelectPartLinkDialogView()
        {
            InitializeComponent();
            _viewSource = (CollectionViewSource)Resources["FilteredProducts"];
            _viewSource.Filter += OnFilter;
        }

        private void OnFilter(object sender, FilterEventArgs filterEventArgs)
        {
            var viewModel = (SelectPartLinkDialogViewModel)DataContext;
            var product = (ProductInfoViewModel)filterEventArgs.Item;

            // Filter by TextBox
            var searchBoxText = SearchTextBox.Text;

            if (!string.IsNullOrEmpty(searchBoxText) &&
                product.DisplayName.IndexOf(searchBoxText, StringComparison.OrdinalIgnoreCase) < 0)
            {
                filterEventArgs.Accepted = false;
                return;
            }

            // Allow
            filterEventArgs.Accepted = true;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshProducts();
        }

        private void RefreshProducts()
        {
            Action action = () =>
            {
                _viewSource.View.Refresh();
            };

            Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }
    }
}
