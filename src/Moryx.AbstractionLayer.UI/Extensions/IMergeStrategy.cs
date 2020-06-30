// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Strategy to merge a collection of models into a collection of view models
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public interface IMergeStrategy<in TModel, TViewModel>
    {
        /// <summary>
        /// Create a new instance from the given model
        /// </summary>
        TViewModel FromModel(TModel model);

        /// <summary>
        /// Update the given view model with the model
        /// </summary>
        /// <param name="viewModel">View model to update</param>
        /// <param name="model">Source object for the update</param>
        void UpdateModel(TViewModel viewModel, TModel model);
    }
}
