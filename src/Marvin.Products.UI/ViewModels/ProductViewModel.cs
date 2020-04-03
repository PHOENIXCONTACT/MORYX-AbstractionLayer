// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.Controls;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;
using Marvin.Tools;

namespace Marvin.Products.UI
{
    /// <summary>
    /// ViewModel representing a fully loaded product
    /// </summary>
    public class ProductViewModel : PropertyChangedBase, IEditableObject, IIdentifiableObject
    {
        private string _name;
        private EntryViewModel _properties;
        private readonly RecipeMergeStrategy _recipeMergeStrategy;
        private readonly PartMergeStrategy _partMergeStrategy;
        private ProductState _state;

        /// <summary>
        /// Underlying model of this view model
        /// </summary>
        public ProductModel Model { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductViewModel"/> class.
        /// </summary>
        public ProductViewModel(ProductModel model)
        {
            Model = model;
            _recipeMergeStrategy = new RecipeMergeStrategy();
            _partMergeStrategy = new PartMergeStrategy();

            CopyFromModel();
        }

        private void CopyFromModel()
        {
            Name = Model.Name;
            State = Model.State;
            Properties = Model.Properties != null
                ? new EntryViewModel(Model.Properties.Clone(true))
                : new EntryViewModel(new List<Entry>());

            if (Model.Parts != null)
                Parts.MergeCollection(Model.Parts, _partMergeStrategy);

            if (Model.Recipes != null)
                Recipes.MergeCollection(Model.Recipes, _recipeMergeStrategy);
        }

        private void CopyToModel()
        {
            Model.Name = Name;
            Model.State = State;
            Model.Properties = Properties.Entry;
            Model.Parts = Parts.Select(p => p.Model).ToArray();
            Model.Recipes = Recipes.Select(r => r.Model).ToArray();
        }

        /// <summary>
        /// Gets the unique identifier of the product.
        /// </summary>
        public long Id => Model.Id;

        /// <summary>
        /// The identifier of the product
        /// </summary>
        public string Identifier => Model.Identifier;

        /// <summary>
        /// Revision of th product
        /// </summary>
        public short Revision => Model.Revision;

        /// <summary>
        /// Gets the full identifier of the product IIIII-RR
        /// </summary>
        public string FullIdentifier => $"{Model.Identifier}-{Model.Revision:D2}";

        /// <summary>
        /// Combination of FullIdentifier and name
        /// </summary>
        public string DisplayName => $"{Model.Identifier}-{Model.Revision:D2} {Model.Name}";

        /// <summary>
        /// Collection of part connectors
        /// </summary>
        public ObservableCollection<PartConnectorViewModel> Parts { get; } = new ObservableCollection<PartConnectorViewModel>();

        /// <summary>
        /// Collection of recipes
        /// </summary>
        public ObservableCollection<RecipeViewModel> Recipes { get; } = new ObservableCollection<RecipeViewModel>();

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the state of the product
        /// </summary>
        public ProductState State
        {
            get { return _state; }
            set
            {
                _state = value;
                NotifyOfPropertyChange();
            }
        }


        /// <summary>
        /// Entry view model of the product properties
        /// </summary>
        public EntryViewModel Properties
        {
            get { return _properties; }
            private set
            {
                _properties = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public void BeginEdit()
        {
            Parts.BeginEdit();
        }

        /// <inheritdoc />
        public void EndEdit()
        {
            Parts.EndEdit();
            CopyToModel();
        }

        /// <inheritdoc />
        public void CancelEdit()
        {
            Parts.CancelEdit();
            CopyFromModel();
        }

        /// <summary>
        /// Updates the internal model and raises NotifyPropertyChanged
        /// </summary>
        /// <param name="productModel">The updated model instance</param>
        public void UpdateModel(ProductModel productModel)
        {
            Model = productModel;

            CopyFromModel();

            NotifyOfPropertyChange(nameof(Name));
            NotifyOfPropertyChange(nameof(Properties));
        }

        private class RecipeMergeStrategy : IMergeStrategy<RecipeModel, RecipeViewModel>
        {
            public RecipeViewModel FromModel(RecipeModel model) => new RecipeViewModel(model);

            public void UpdateModel(RecipeViewModel viewModel, RecipeModel model) => viewModel.UpdateModel(model);
        }

        private class PartMergeStrategy : IMergeStrategy<PartConnector, PartConnectorViewModel>
        {
            public PartConnectorViewModel FromModel(PartConnector model) => new PartConnectorViewModel(model);

            public void UpdateModel(PartConnectorViewModel viewModel, PartConnector model) => viewModel.UpdateModel(model);
        }
    }
}
