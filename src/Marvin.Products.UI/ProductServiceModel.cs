using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Marvin.Logging;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;
using Marvin.Tools;
using Marvin.Tools.Wcf;

namespace Marvin.Products.UI
{
    internal class ProductServiceModel : HttpServiceConnectorBase<ProductInteractionClient, IProductInteraction>, IProductServiceModel
    {
        private readonly IModuleLogger _logger;

        public ProductServiceModel(IWcfClientFactory clientFactory, IModuleLogger logger)
        {
            _logger = logger;
            ClientFactory = clientFactory;
            Structure = new ObservableCollection<ProductStructureEntry>();
        }

        protected override string MinServerVersion => "1.1.2.0";

        protected override string ClientVersion => "1.1.2.0";

        public ObservableCollection<ProductStructureEntry> Structure { get; }

        public ProductCustomization Customization { get; private set; }

        protected override async void ClientCallback(ConnectionState state, ProductInteractionClient client)
        {
            base.ClientCallback(state, client);

            _logger.Log(LogLevel.Info, "Product interaction service changed state to {0}", state);
            if (state != ConnectionState.Success)
                return;

            try
            {
                await LoadStructure();
                Customization = await WcfClient.GetCustomizationAsync();
            }
            catch (Exception ex)
            {
                //ToDo: Maybe the wcf is ready and connected before the underlying components are ready.
                _logger.LogException(LogLevel.Error, ex, "Fail to get basic information after state changed to connected!");
            }
        }

        private async Task LoadStructure()
        {
            var structure = await WcfClient.GetProductStructureAsync();
            Structure.Clear();
            Structure.AddRange(structure);

            RaiseStructureUpdated();
        }

        public void UpdateStructure() => Task.Run(LoadStructure);
        public Task<ProductModel[]> GetAll() =>
            WcfClient.GetAllProductsAsync();

        public Task<ProductModel> GetDetails(long id) =>
            WcfClient.GetProductDetailsAsync(id);

        public Task<ProductModel> Save(ProductModel product) =>
            WcfClient.SaveProductAsync(product);

        public Task<Entry> UpdateParameters(string importer, Entry currentParameters) =>
            WcfClient.UpdateParametersAsync(importer, currentParameters);

        public Task<ProductModel> ImportProduct(string importerName, Entry parameters) =>
            WcfClient.ImportProductAsync(importerName, parameters);

        public Task<ProductModel[]> RemoveProduct(long id) =>
            WcfClient.DeleteProductAsync(id);

        public Task<RecipeModel[]> GetProductionRecipes(long productId) =>
            WcfClient.GetRecipesAsync(productId);

        public Task<RecipeModel> CreateProductionRecipe(long productId, long workplanId, string name) =>
            WcfClient.CreateProductionRecipeAsync(productId, workplanId, name);

        public Task<bool> SaveProductionRecipe(RecipeModel recipe) =>
            WcfClient.SaveProductionRecipeAsync(recipe);

        public Task<RecipeModel> GetRecipe(long recipeId) =>
            WcfClient.GetRecipeAsync(recipeId);

        public Task<WorkplanModel[]> GetWorkplans() =>
            WcfClient.GetWorkplansAsync();

        public WorkplanModel GetWorkplanById(long workplanId) =>
            WcfClient.GetWorkplan(workplanId);

        public Task<ProductModel> CreateRevision(long productId, short revisionNo, string comment) =>
            WcfClient.CreateRevisionAsync(productId, revisionNo, comment);

        public Task<ProductRevisionEntry[]> GetProductRevisions(string identifier) =>
            WcfClient.GetProductRevisionsAsync(identifier);

        public event EventHandler StructureUpdated;
        private void RaiseStructureUpdated() =>
            StructureUpdated?.Invoke(this, EventArgs.Empty);
    }
}