// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI
{
    public class ReferenceTypeViewModel : PropertyChangedBase
    {
        public ReferenceTypeModel Model { get; }

        public ResourceTypeViewModel ResourceType { get; }

        public string DisplayName => Model.DisplayName;

        public string Description => Model.Description;

        public bool IsCollection => Model.IsCollection;

        public bool IsRequired => Model.IsRequired;

        public ReferenceTypeViewModel(ResourceTypeViewModel resourceType, ReferenceTypeModel model)
        {
            ResourceType = resourceType;
            Model = model;
        }
    }
}
