// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Marvin.Resources.UI.ResourceService;

namespace Marvin.Resources.UI
{
    /// <summary>
    /// View model for the resource type
    /// </summary>
    public class ResourceTypeViewModel : PropertyChangedBase
    {
        private readonly ResourceTypeModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceTypeViewModel"/> class.
        /// </summary>
        public ResourceTypeViewModel(ResourceTypeModel model)
        {
            _model = model;
            Constructors = model.Constructors.Select(c => new ConstructorViewModel(c)).ToArray();

            if (Constructors.Length > 0)
                Constructors[0].IsSelected = true;

            DerivedTypes = model.DerivedTypes.Select(d => new ResourceTypeViewModel(d)).ToArray();
            References = model.References.Select(r => new ReferenceTypeViewModel(this, r)).ToArray();
        }

        /// <summary>
        /// The name of the resource
        /// </summary>
        public string Name => _model.Name;

        /// <summary>
        /// Display name of the type
        /// </summary>
        public string DisplayName => _model.DisplayName;

        /// <summary>
        /// Description of the type
        /// </summary>
        public string Description => _model.Description;

        /// <summary>
        /// Flag if this type can be instantiated
        /// </summary>
        public bool Creatable => _model.Creatable;

        /// <summary>
        /// Constructors of this type
        /// </summary>
        public ConstructorViewModel[] Constructors { get; }

        /// <summary>
        /// The children of the current tree item
        /// </summary>
        public ResourceTypeViewModel[] DerivedTypes { get; }

        /// <summary>
        /// Type model for the references
        /// </summary>
        public ReferenceTypeViewModel[] References { get; }
    }
}
