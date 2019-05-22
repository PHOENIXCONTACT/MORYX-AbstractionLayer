using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C4I;
using Marvin.ClientFramework.Commands;
using Marvin.ClientFramework.Dialog;
using Marvin.ClientFramework.Tasks;
using Marvin.Container;

namespace Marvin.Products.UI.Interaction
{
    internal interface IAddRecipeDialog : IDialogScreen
    {

    }

    [Plugin(LifeCycle.Transient, typeof(IAddRecipeDialog))]
    internal class AddRecipeDialogViewModel : DialogScreen, IAddRecipeDialog
    {
        #region Dependency Injections

        /// <summary>
        /// Injected products controller
        /// </summary>
        public IProductServiceModel ProductServiceModel { get; set; }

        #endregion

        #region Fields and Properties

        private readonly long _productId;
        private WorkplanViewModel _selectedWorkplan;
        private TaskNotifier _taskNotifier;

        public override string DisplayName => "Add a new recipe";

        public AsyncCommand CreateCmd { get; }

        public RelayCommand CloseCmd { get; }

        public string ProductName { get; }

        public WorkplanViewModel[] Workplans { get; }

        public string RecipeName { get; set; }

        public TaskNotifier TaskNotifier
        {
            get { return _taskNotifier; }
            private set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange(nameof(TaskNotifier));
            }
        }

        public WorkplanViewModel SelectedWorkplan
        {
            get { return _selectedWorkplan; }
            set
            {
                _selectedWorkplan = value;
                NotifyOfPropertyChange(nameof(SelectedWorkplan));
            }
        }

        #endregion

        public AddRecipeDialogViewModel(string productName, long productId, ICollection<WorkplanViewModel> workplans)
        {
            ProductName = productName;
            _productId = productId;
            Workplans = workplans.ToArray();
            SelectedWorkplan = Workplans.FirstOrDefault();

            CloseCmd = new RelayCommand(Close);
            CreateCmd = new AsyncCommand(Create, CanCreate, true);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        private void Close(object obj)
        {
            TryClose(false);
        }

        private bool CanCreate(object arg)
        {
            return SelectedWorkplan != null && !string.IsNullOrWhiteSpace(RecipeName);
        }

        private async Task Create(object arg)
        {
            var createTask =  ProductServiceModel.CreateProductionRecipe(_productId, _selectedWorkplan.Id, RecipeName);
            TaskNotifier = new TaskNotifier(createTask);

            await createTask;

            TryClose(true);
        }
    }
}