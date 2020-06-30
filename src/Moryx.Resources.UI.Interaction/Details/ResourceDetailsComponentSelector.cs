// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Linq;
using System.Reflection;
using Moryx.AbstractionLayer.UI;
using Moryx.Container;
using Moryx.Resources.UI.ResourceService;

namespace Moryx.Resources.UI.Interaction
{
    /// <summary>
    /// Component selector for resource view models
    /// </summary>
    [Plugin(LifeCycle.Singleton)]
    internal class ResourceDetailsComponentSelector : DetailsComponentSelector<IResourceDetails>
    {
        /// <summary>
        /// Service model to support inherited types
        /// </summary>
        public IResourceServiceModel ServiceModel { get; set; }

        public ResourceDetailsComponentSelector(IContainer container) : base(container)
        {
        }

        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            var groupType = arguments.FirstOrDefault() as string;
            // If no name was given use the default type
            if (groupType == null)
                return Registrations[DetailsConstants.DefaultType];

            // Directly return the default view if it is known
            if (Registrations.ContainsKey(groupType))
                return Registrations[groupType];

            // Start from the current type going upwards the type tree looking for a custom ui
            var typeModel = FindType(groupType, ServiceModel.TypeTree);
            while (typeModel != null)
            {
                if (Registrations.ContainsKey(typeModel.Name))
                    return Registrations[typeModel.Name];
                typeModel = typeModel.BaseType;
            }

            // If all failed use the default
            return Registrations[DetailsConstants.DefaultType];
        }

        private static ResourceTypeModel FindType(string searchedType, ResourceTypeModel typeBranch)
        {
            if (typeBranch.Name == searchedType)
                return typeBranch;

            foreach (var typeModel in typeBranch.DerivedTypes)
            {
                var match = FindType(searchedType, typeModel);
                if (match != null)
                    return match;
            }

            return null;
        }
    }
}
