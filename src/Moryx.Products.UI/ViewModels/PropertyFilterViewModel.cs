using Caliburn.Micro;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI
{
    public class PropertyFilterViewModel : PropertyChangedBase
    {
        public Serialization.Entry Property { get; set; }

        public PropertyFilterOperator Operator { get; set; }

        public PropertyFilterViewModel(Serialization.Entry model)
        {
            Property = model;
        }
    }
}