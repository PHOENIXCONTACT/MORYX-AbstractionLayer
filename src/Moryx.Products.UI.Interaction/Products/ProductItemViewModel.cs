// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.UI;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI.Interaction
{
    internal class TypeItemViewModel : TreeItemViewModel
    {
        public ProductDefinitionModel Model { get; private set; }

        public string TypeName => Model.Name;

        public override long Id => 0;

        /// <inheritdoc />
        public override string DisplayName => Model.DisplayName;

        public TypeItemViewModel(ProductDefinitionModel model)
        {
            Model = model;
            UpdateModel(model);
        }

        public void UpdateModel(ProductDefinitionModel model)
        {
            Model = model;
            NotifyOfPropertyChange(DisplayName);
        }
    }

    internal class ProductItemViewModel : TreeItemViewModel
    {
        /// <inheritdoc />
        public override string DisplayName => Product.DisplayName;

        public ProductInfoViewModel Product { get; private set; }

        public override long Id => Product.Id;

        public string Identifier => Product.Identifier;

        public ProductItemViewModel(ProductModel model)
        {
            UpdateModel(model);
        }

        public void UpdateModel(ProductModel model)
        {
            Product = new ProductInfoViewModel(model);

            NotifyOfPropertyChange(nameof(DisplayName));
            NotifyOfPropertyChange(nameof(Identifier));
            NotifyOfPropertyChange(nameof(Product));
        }
    }
}
