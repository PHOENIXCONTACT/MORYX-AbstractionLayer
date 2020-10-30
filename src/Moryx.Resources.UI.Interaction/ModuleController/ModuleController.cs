// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Windows.Media;
using C4I;
using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.ClientFramework;
using Moryx.Logging;
using Moryx.Resources.UI.Interaction.Properties;
using Moryx.Tools.Wcf;

namespace Moryx.Resources.UI.Interaction
{
    /// <summary>
    /// Module controller of the Resources UI.
    /// </summary>
    [ClientModule(ModuleName)]
    public class ModuleController : WorkspaceModuleBase<ModuleConfig>
    {
        internal const string ModuleName = "Resources";

        /// <inheritdoc />
        public override Geometry Icon => CommonShapeFactory.GetShapeGeometry(CommonShapeType.Cells);

        ///
        protected override void OnInitialize()
        {
            DisplayName = Strings.ModuleController_DisplayName;

            // Register aspect factory
            Container.Register<IAspectFactory>();

            // Load resource aspects to this container
            Container.Register<IAspectConfigurator, AspectConfiguratorViewModel>();
            Container.Register<IAspectConfiguratorFactory>();
            Container.LoadComponents<IResourceAspect>();

            // Load ResourceDetails to this container
            Container.LoadComponents<IResourceDetails>();

            // Register and start service model
            var clientFactory = Container.Resolve<IWcfClientFactory>();
            var logger = Container.Resolve<IModuleLogger>();
            var serviceModel = Resources.CreateServiceModel(clientFactory, logger);

            Container.SetInstance(serviceModel);

            serviceModel.Start();
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {

        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            if(close)
                Container.Resolve<IResourceServiceModel>().Stop();
        }

        /// <inheritdoc />
        protected override IModuleWorkspace OnCreateWorkspace()
        {
            return Container.Resolve<IModuleWorkspace>(InteractionWorkspaceViewModel.WorkspaceName);
        }

        /// <inheritdoc />
        protected override void OnDestroyWorkspace(IModuleWorkspace workspace)
        {

        }
    }
}
