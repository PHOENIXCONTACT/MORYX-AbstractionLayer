// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI
{
    public class ResourceInfoViewModel : PropertyChangedBase, IResourceViewModel
    {
        public ResourceModel Model { get; private set; }

        public ResourceInfoViewModel(ResourceModel model)
        {
            UpdateModel(model);
        }

        public void UpdateModel(ResourceModel model)
        {
            Model = model;
            NotifyOfPropertyChange(nameof(Name));
            NotifyOfPropertyChange(nameof(Description));
        }

        public long Id => Model.Id;

        public string Name => Model.Name;

        public string Description => Model.Description;

        public string TypeName => Model.Type;
    }
}
