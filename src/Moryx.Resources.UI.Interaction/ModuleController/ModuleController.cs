// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using System.Windows.Media;
using Moryx.WpfToolkit;
using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.ClientFramework;
using Moryx.Configuration;
using Moryx.Logging;
using Moryx.Resources.UI.Interaction.Properties;

namespace Moryx.Resources.UI.Interaction
{
    /// <summary>
    /// Module controller of the Resources UI.
    /// </summary>
    [ClientModule("Resources")]
    public class ModuleController : WorkspaceModuleBase<ModuleConfig>
    {
        /// <inheritdoc />
        public override Geometry Icon => CommonShapeFactory.GetShapeGeometry(CommonShapeType.Cells);

        /// <summary>
        /// Config manager to get configurations
        /// </summary>
        public IConfigManager ConfigManager { get; set; }

        ///
        protected override Task OnInitializeAsync()
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
            var runtimeConfig = ConfigManager.GetConfiguration<RuntimeConfig>();
            var proxyConfig = ConfigManager.GetConfiguration<ProxyConfig>();
            var logger = Container.Resolve<IModuleLogger>();

            IResourceServiceModel serviceModel = new ResourceServiceModel(runtimeConfig.Host, runtimeConfig.Port, proxyConfig, logger);
            Container.SetInstance(serviceModel);

            serviceModel.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override Task OnActivateAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override Task OnDeactivateAsync(bool close)
        {
            if(close)
                Container.Resolve<IResourceServiceModel>().Stop();

            return Task.CompletedTask;
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
