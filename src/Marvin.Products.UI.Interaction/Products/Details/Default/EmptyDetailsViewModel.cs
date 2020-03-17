// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using System.Windows.Media;
using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI.Interaction
{
    [ProductDetailsRegistration(DetailsConstants.EmptyType)]
    internal class EmptyDetailsViewModel : EmptyDetailsViewModelBase, IProductDetails
    {
        public Task Load(long productId) => SuccessTask;

        public ProductViewModel EditableObject => null;

        public Geometry Icon => ModuleController.IconGeometry;

        public override bool CanBeginEdit() => false;
    }
}
