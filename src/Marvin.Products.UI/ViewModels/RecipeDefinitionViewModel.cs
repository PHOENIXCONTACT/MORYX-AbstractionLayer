// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Marvin.Products.UI.ProductService;

namespace Marvin.Products.UI
{
    /// <summary>
    /// ViewModel representing a recipe type
    /// </summary>
    public class RecipeDefinitionViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Underlying model
        /// </summary>
        public RecipeDefinitionModel Model { get; private set; }

        /// <summary>
        /// DisplayName of the recipe type
        /// </summary>
        public string DisplayName => Model.DisplayName;

        /// <summary>
        /// Indicator if the recipe has a workplan
        /// </summary>
        public bool HasWorkplans => Model.HasWorkplans;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeDefinitionViewModel"/> class.
        /// </summary>
        public RecipeDefinitionViewModel(RecipeDefinitionModel model)
        {
            Model = model;
            UpdateModel(model);
        }

        public void UpdateModel(RecipeDefinitionModel model)
        {
            Model = model;

            NotifyOfPropertyChange(nameof(HasWorkplans));
            NotifyOfPropertyChange(nameof(DisplayName));
        }
    }
}
