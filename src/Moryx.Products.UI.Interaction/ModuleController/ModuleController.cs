// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Threading.Tasks;
using System.Windows.Media;
using Moryx.WpfToolkit;
using Moryx.AbstractionLayer.UI.Aspects;
using Moryx.ClientFramework;
using Moryx.Configuration;
using Moryx.Logging;
using Moryx.Products.UI.Interaction.Properties;
using ProxyConfig = Moryx.ClientFramework.ProxyConfig;

namespace Moryx.Products.UI.Interaction
{
    /// <summary>
    /// Module controller handling the lifecycle of the module
    /// </summary>
    [ClientModule("Products", typeof(IRecipeWorkspaceProvider))]
    public class ModuleController : WorkspaceModuleBase<ModuleConfig>, IRecipeWorkspaceProvider
    {
        internal static Geometry IconGeometry = MdiShapeFactory.GetShapeGeometry(MdiShapeType.Developer_Board);

        /// <inheritdoc />
        public override Geometry Icon => IconGeometry;

        /// <summary>
        /// Config manager to get configurations
        /// </summary>
        public IConfigManager ConfigManager { get; set; }

        /// <summary>
        /// Initializes the module
        /// </summary>
        protected override Task OnInitializeAsync()
        {
            DisplayName = Strings.ModuleController_DisplayName;

            // Register aspect factory
            Container.Register<IAspectFactory>();

            // Load ResourceDetails and RecipeDetails to the current module container
            Container.Register<IAspectConfigurator, AspectConfiguratorViewModel>();
            Container.Register<IAspectConfiguratorFactory>();
            Container.LoadComponents<IProductDetails>();
            Container.LoadComponents<IRecipeDetails>();
            Container.LoadComponents<IProductAspect>();

            // Register and start service model
            var runtimeConfig = ConfigManager.GetConfiguration<RuntimeConfig>();
            var proxyConfig = ConfigManager.GetConfiguration<ProxyConfig>();
            var logger = Container.Resolve<IModuleLogger>();

            IProductServiceModel serviceModel = new ProductServiceModel(runtimeConfig.Host, runtimeConfig.Port, proxyConfig, logger);
            Container.SetInstance(serviceModel);

            serviceModel.Start();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Will be called when the module will be selected
        /// </summary>
        protected override Task OnActivateAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Will be called when the module will be deactivated
        /// </summary>
        protected override Task OnDeactivateAsync(bool close)
        {
            if (close)
                Container.Resolve<IProductServiceModel>().Stop();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Will be called by selecting the module
        /// </summary>
        protected override IModuleWorkspace OnCreateWorkspace()
        {
            return Container.Resolve<IModuleWorkspace>(ProductsWorkspaceViewModel.WorkspaceName);
        }

        /// <summary>
        /// Will destroy the given workspace
        /// </summary>
        protected override void OnDestroyWorkspace(IModuleWorkspace workspace)
        {

        }

        /// <inheritdoc />
        IRecipeWorkspace IRecipeWorkspaceProvider.CreateWorkspace(string title, params long[] recipeIds)
        {
            var recipeEditorFactory = Container.Resolve<IRecipeWorkspaceFactory>();
            return recipeEditorFactory.CreateRecipeWorkspace(title, recipeIds);
        }
    }
}
