// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Moryx.Container;

namespace Moryx.AbstractionLayer.UI
{
    /// <summary>
    /// Component selector for specialized plugins defined by runtime data
    /// </summary>
    public abstract class DetailsComponentSelector<TDetailsType> : DefaultTypedFactoryComponentSelector
        where TDetailsType : class
    {
        /// <summary>
        /// Registrations of view models for different types
        /// </summary>
        protected Dictionary<string, string> Registrations { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsComponentSelector{TDetailsType}"/> class.
        /// </summary>
        protected DetailsComponentSelector(IContainer container)
        {
            Registrations = (from type in container.GetRegisteredImplementations(typeof(TDetailsType))
                let att = type.GetCustomAttribute<DetailsRegistrationAttribute>()
                where att != null
                select new
                {
                    GroupName = att.TypeName,
                    PluginType = type.FullName
                }).ToDictionary(e => e.GroupName, e => e.PluginType);
        }

        ///
        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            return arguments.FirstOrDefault() is string groupType && Registrations.ContainsKey(groupType)
                ? Registrations[groupType]
                : Registrations[DetailsConstants.DefaultType];
        }

        ///
        protected override Func<IKernelInternal, IReleasePolicy, object> BuildFactoryComponent(MethodInfo method, string componentName, Type componentType, Arguments additionalArguments)
        {
            return delegate (IKernelInternal @internal, IReleasePolicy policy)
            {
                var component = base.BuildFactoryComponent(method, componentName, componentType, additionalArguments)(@internal, policy);

                var baseViewModel = component as IDetailsViewModel;
                baseViewModel?.Initialize(componentName);

                return component;
            };
        }
    }
}
