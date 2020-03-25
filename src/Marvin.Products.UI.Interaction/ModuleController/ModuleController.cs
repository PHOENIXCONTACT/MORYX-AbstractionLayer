using System.Windows.Media;
using C4I;
using Marvin.AbstractionLayer.UI.Aspects;
using Marvin.ClientFramework;
using Marvin.Logging;
using Marvin.Products.UI.Interaction.Properties;
using Marvin.Tools.Wcf;

namespace Marvin.Products.UI.Interaction
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
        /// Initializes the module
        /// </summary>
        protected override void OnInitialize()
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
            var clientFactory = Container.Resolve<IWcfClientFactory>();
            var logger = Container.Resolve<IModuleLogger>();

            var serviceModel = Products.CreateServiceModel(clientFactory, logger);

            Container.SetInstance(serviceModel);

            serviceModel.Start();
        }

        /// <summary>
        /// Will be called when the module will be selected
        /// </summary>
        protected override void OnActivate()
        {

        }

        /// <summary>
        /// Will be called when the module will be deactivated
        /// </summary>
        protected override void OnDeactivate(bool close)
        {
            if (close)
                Container.Resolve<IProductServiceModel>().Stop();
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