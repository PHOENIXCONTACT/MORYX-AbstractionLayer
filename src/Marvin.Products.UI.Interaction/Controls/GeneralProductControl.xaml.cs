using System.Windows;
using System.Windows.Controls;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Interaction logic for GeneralProductControl.xaml
    /// </summary>
    public partial class GeneralProductControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralProductControl"/> class.
        /// </summary>>
        public GeneralProductControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Dependency property for the <see cref="Product"/>
        /// </summary>
        public static readonly DependencyProperty ProductProperty = DependencyProperty.Register(
            "Product", typeof(IProductHead), typeof(GeneralProductControl), new PropertyMetadata(default(IProductHead)));

        /// <summary>
        /// The resource property
        /// </summary>
        public IProductHead Product
        {
            get { return (IProductHead)GetValue(ProductProperty); }
            set { SetValue(ProductProperty, value); }
        }

        /// <summary>
        /// Dependency property for the <see cref="IsEditMode"/>
        /// </summary>
        public static readonly DependencyProperty IsEditModeProperty = DependencyProperty.Register(
            "IsEditMode", typeof(bool), typeof(GeneralProductControl), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Edit Mode Property 
        /// </summary>
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }
    }
}
