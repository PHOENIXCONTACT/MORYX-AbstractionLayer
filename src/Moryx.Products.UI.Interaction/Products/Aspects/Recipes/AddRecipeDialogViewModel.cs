// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Moryx.ClientFramework.Commands;
using Moryx.ClientFramework.Dialog;
using Moryx.ClientFramework.Tasks;
using Moryx.Products.UI.Interaction.Properties;

namespace Moryx.Products.UI.Interaction.Aspects
{
    internal class AddRecipeDialogViewModel : DialogScreen
    {
        #region Fields and Properties

        public override string DisplayName => Strings.AddRecipeDialogViewModel_DisplayName;

        private readonly IProductServiceModel _productServiceModel;
        private TaskNotifier _taskNotifier;
        private string _errorMessage;

        /// <summary>
        /// Command to create the recipe
        /// </summary>
        public ICommand CreateCmd { get; }

        /// <summary>
        /// Command to close the dialog
        /// </summary>
        public ICommand CloseCmd { get; }

        /// <summary>
        /// Collection of possible workplans
        /// </summary>
        public WorkplanViewModel[] Workplans { get; }

        /// <summary>
        /// Collection of possible recipe types
        /// </summary>
        public RecipeDefinitionViewModel[] PossibleRecipeTypes { get; private set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        public string Name { get; set; }

        private RecipeDefinitionViewModel _selectedRecipeDefinition;
        /// <summary>
        /// Selected type of recipe
        /// </summary>
        public RecipeDefinitionViewModel SelectedRecipeDefinition
        {
            get => _selectedRecipeDefinition;
            set
            {
                _selectedRecipeDefinition = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Selected workplan for this recipe
        /// </summary>
        public WorkplanViewModel SelectedWorkplan { get; set; }

        /// <summary>
        /// Simple notifier to display a busy indicator
        /// </summary>
        public TaskNotifier TaskNotifier
        {
            get => _taskNotifier;
            private set
            {
                _taskNotifier = value;
                NotifyOfPropertyChange(nameof(TaskNotifier));
            }
        }

        /// <summary>
        /// Displays general error messages if new revision creation is failed
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Created prototype
        /// </summary>
        public RecipeViewModel RecipePrototype { get; private set; }

        #endregion

        public AddRecipeDialogViewModel(IProductServiceModel productServiceModel,
            IEnumerable<WorkplanViewModel> workplans)
        {
            _productServiceModel = productServiceModel;
            Workplans = workplans.ToArray();
            SelectedWorkplan = Workplans.FirstOrDefault();

            CloseCmd = new AsyncCommand(Close, CanClose, true);
            CreateCmd = new AsyncCommand(Create, CanCreate, true);
        }

        private bool CanClose(object obj) =>
            TaskNotifier == null || TaskNotifier != null && TaskNotifier.IsCompleted;

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            var loadingTask = Task.Run(async delegate
            {
                try
                {
                    var customization = await _productServiceModel.GetCustomization().ConfigureAwait(false);
                    Execute.OnUIThread(delegate
                    {
                        PossibleRecipeTypes = customization.RecipeTypes.Select(r => new RecipeDefinitionViewModel(r)).ToArray();
                        SelectedRecipeDefinition = PossibleRecipeTypes.FirstOrDefault();
                    });
                }
                catch (Exception e)
                {
                    Execute.OnUIThread(() => ErrorMessage = e.Message);
                }
            }, cancellationToken);

            TaskNotifier = new TaskNotifier(loadingTask);
        }

        private bool CanCreate(object arg)
        {
            if (SelectedRecipeDefinition == null)
                return false;

            if (SelectedRecipeDefinition.HasWorkplans && SelectedWorkplan == null)
                return false;

            if (string.IsNullOrWhiteSpace(Name))
                return false;

            return true;
       }

        private async Task Create(object arg)
        {
            ErrorMessage = string.Empty;

            try
            {
                var createTask =  _productServiceModel.CreateRecipe(SelectedRecipeDefinition.Model.Name);
                TaskNotifier = new TaskNotifier(createTask);

                var newRecipe = await createTask;

                RecipePrototype = new RecipeViewModel(newRecipe)
                {
                    Name = Name
                };

                if (SelectedRecipeDefinition.HasWorkplans)
                    RecipePrototype.Model.WorkplanId = SelectedWorkplan.Id;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }

            await TryCloseAsync(true);
        }

        private Task Close(object obj) =>
            TryCloseAsync(false);
    }
}
