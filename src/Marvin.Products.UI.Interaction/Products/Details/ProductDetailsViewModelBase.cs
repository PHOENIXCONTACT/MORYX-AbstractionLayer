// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Marvin.AbstractionLayer.UI;
using Marvin.AbstractionLayer.UI.Aspects;
using Marvin.Products.UI.Interaction.Properties;
using Marvin.Tools;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// Base class for the product details view model
    /// </summary>
    public abstract class ProductDetailsViewModelBase : EditModeViewModelBase<ProductViewModel>, IProductDetails
    {
        #region Dependency Injection

        /// <summary>
        /// Service model to load additional information from the server
        /// </summary>
        public IProductServiceModel ProductServiceModel { get; set; }

        /// <summary>
        /// Factory to create aspects
        /// </summary>
        public IAspectFactory AspectFactory { get; set; }

        /// <summary>
        /// Configuration of the module
        /// </summary>
        public ModuleConfig Config { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// View models of aspects from this product
        /// </summary>
        public AspectConductorViewModel Aspects { get; } = new AspectConductorViewModel(Strings.ProductDetailsViewModelBase_No_relevant_aspects);

        /// <summary>
        /// Property defining if aspects should be loaded or not
        /// </summary>
        protected virtual bool AspectUsage => false;

        #endregion

        /// <inheritdoc />
        public void Initialize(string typeName)
        {
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            base.OnActivate();
            ScreenExtensions.TryActivate(Aspects);
        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            // Copy aspects, because they are cleared on deactivation
            var aspects = Aspects.Items.Cast<IProductAspect>().ToArray();
            ScreenExtensions.TryDeactivate(Aspects, close);

            if (close)
                aspects.ForEach(aspect => AspectFactory.Destroy(aspect));
        }

        /// <inheritdoc />
        public async Task Load(long productId)
        {
            Execute.OnUIThread(() => IsBusy = true);

            try
            {
                var model = await ProductServiceModel.GetProductDetails(productId);
                EditableObject = new ProductViewModel(model);

                if (AspectUsage)
                {
                    var typedAspects = Config.AspectConfigurations.FirstOrDefault(ac => ac.TypeName == model.Type);
                    List<AspectConfiguration> aspectConfigurations;
                    if (typedAspects == null || typedAspects.Aspects.Count == 0)
                        aspectConfigurations = Config.DefaultAspects;
                    else
                        aspectConfigurations = typedAspects.Aspects;

                    // Load aspects
                    var aspects = aspectConfigurations.Select(ca => (IProductAspect)AspectFactory.Create(ca.PluginName))
                        .Where(a => a.IsRelevant(EditableObject)).ToArray();

                    var aspectLoadTasks = new List<Task>(aspects.Select(aspect => aspect.Load(EditableObject)));

                    await Task.WhenAll(aspectLoadTasks);
                    Aspects.Items.AddRange(aspects);
                }
            }
            catch
            {
                //TODO
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <inheritdoc />
        public override void Validate(ICollection<ValidationResult> validationErrors)
        {
            base.Validate(validationErrors);
            Aspects.Items.OfType<IProductAspect>().ForEach(a => a.Validate(validationErrors));
        }

        ///
        public override async Task Save()
        {
            IsBusy = true;

            try
            {
                foreach (var aspect in Aspects.Items.Cast<IProductAspect>())
                    await aspect.Save();

                var updated = await ProductServiceModel.SaveProduct(EditableObject.Model);
                EditableObject.UpdateModel(updated);
            }
            catch
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <inheritdoc />
        public override void BeginEdit()
        {
            Aspects.BeginEdit();
            base.BeginEdit();
        }

        /// <inheritdoc />
        public override void EndEdit()
        {
            Aspects.EndEdit();
            base.EndEdit();
        }

        /// <inheritdoc />
        public override void CancelEdit()
        {
            Aspects.CancelEdit();
            base.CancelEdit();
        }
    }
}
