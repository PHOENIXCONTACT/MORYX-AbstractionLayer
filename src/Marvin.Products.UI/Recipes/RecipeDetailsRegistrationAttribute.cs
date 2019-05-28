using Marvin.AbstractionLayer.UI;

namespace Marvin.Products.UI.Recipes
{
    /// <summary>
    /// Registration attribute to register <see cref="IRecipeDetails"/> for a product group
    /// </summary>
    public class RecipeDetailsRegistrationAttribute : DetailsRegistrationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeDetailsRegistrationAttribute"/> class.
        /// </summary>
        public RecipeDetailsRegistrationAttribute(string typeName)
            : base(typeName, typeof(IRecipeDetails))
        {
        }
    }
}