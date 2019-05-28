using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Marvin.AbstractionLayer.UI;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;
using Marvin.Tools.Wcf;

namespace Marvin.Products.UI
{
    /// <summary>
    /// Model interface for product interaction
    /// </summary>
    public interface IProductServiceModel : IInteractionController, IHttpServiceConnector
    {
        /// <summary>
        /// Represents the product structure tree
        /// </summary>
        ObservableCollection<ProductStructureEntry> Structure { get; }

        /// <summary>
        /// Customization of this application
        /// </summary>
        ProductCustomization Customization { get; }

        /// <summary>
        /// Will updated the product structure tree
        /// </summary>
        void UpdateStructure();

        /// <summary>
        /// Returns all products
        /// </summary>
        Task<ProductModel[]> GetAll();

        /// <summary>
        /// Will return a full product
        /// </summary>
        Task<ProductModel> GetDetails(long id);

        /// <summary>
        /// Will save the product
        /// </summary>
        Task<ProductModel> Save(ProductModel product);

        /// <summary>
        /// Update importer parameters
        /// </summary>
        /// <param name="importer">Name of the importer</param>
        /// <param name="currentParameters">List of parameters</param>
        /// <returns>List of importer parameters</returns>
        Task<Entry> UpdateParameters(string importer, Entry currentParameters);

        /// <summary>
        /// Import product
        /// </summary>
        /// <param name="importerName">Name of the importer</param>
        /// <param name="parameters">List of parameters</param>
        /// <returns>Imported product</returns>
        Task<ProductModel> ImportProduct(string importerName, Entry parameters);

        /// <summary>
        /// Remove a product from the database
        /// </summary>
        Task<ProductModel[]> RemoveProduct(long id);

        /// <summary>
        /// Create product revision
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="revisionNo">New revision number</param>
        /// <param name="comment">Comment for new revision</param>
        /// <returns>Created product revision</returns>
        Task<ProductModel> CreateRevision(long productId, short revisionNo, string comment);

        /// <summary>
        /// Gets product revisions
        /// </summary>
        /// <param name="identifier">Product identifier</param>
        /// <returns>Array of ProductRevisionEntry</returns>
        Task<ProductRevisionEntry[]> GetProductRevisions(string identifier);

        /// <summary>
        /// Gets all recipes for the given product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Array of RecipeModel</returns>
        Task<RecipeModel[]> GetProductionRecipes(long productId);

        /// <summary>
        /// Creates a new production recipe
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="workplanId">Workplan identifier</param>
        /// <param name="name">Name of new recipe</param>
        /// <returns>Created RecipeModel</returns>
        Task<RecipeModel> CreateProductionRecipe(long productId, long workplanId, string name);

        /// <summary>
        /// Saves a production recipe instance
        /// </summary>
        /// <param name="recipe">Recipe instance to be saved</param>
        /// <returns>True if recipe was saved otherwise false</returns>
        Task<bool> SaveProductionRecipe(RecipeModel recipe);

        /// <summary>
        /// Gets the recipe by the give id
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <returns><see cref="RecipeModel"/></returns>
        Task<RecipeModel> GetRecipe(long recipeId);

        /// <summary>
        /// Gets all workplans
        /// </summary>
        /// <returns>Array of WorkplanModel</returns>
        Task<WorkplanModel[]> GetWorkplans();

        /// <summary>
        /// Retrieves a <see cref="WorkplanModel"/> by id
        /// </summary>
        WorkplanModel GetWorkplanById(long workplanId);

        /// <summary>
        /// Event which will be raised if the product structure will be updated
        /// </summary>
        event EventHandler StructureUpdated;
    }
}