using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Marvin.Controls;
using Marvin.Products.UI.ProductService;
using Marvin.Serialization;

namespace Marvin.Products.UI.Interaction
{
    /// <summary>
    /// View model for recipes of a product
    /// </summary>
    public class RecipeViewModel : PropertyChangedBase, IEditableObject
    {
        private string _name;
        private string _type;
        private int _revision;
        private long _workplanId;
        private RecipeClassificationModel _classification;

        private WorkplanViewModel _workplanModel;
        private EntryViewModel _ingredients;

        /// <summary>
        /// Underlying model for this view model
        /// </summary>
        public RecipeModel Model { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public RecipeViewModel()
        {
        }

        /// <summary>
        /// Default constructor for this view model
        /// </summary>
        public RecipeViewModel(RecipeModel model, WorkplanViewModel workplan)
        {
            Model = model;
            Workplan = workplan;

            CopyFromModel();
        }

        internal void Initialize(RecipeModel model)
        {
            Model = model;
            CopyFromModel();
        }

        /// <summary>
        /// Updates the underlying models
        /// </summary>
        public void UpdateModel(RecipeModel recipeModel, WorkplanViewModel workplan)
        {
            Model = recipeModel;
            Workplan = workplan;
            CopyFromModel();
        }

        /// <summary>
        /// Copy all model information to the defined view model properties
        /// </summary>
        protected virtual void CopyFromModel()
        {
            Name = Model.Name;
            Type = Model.Type;
            Classification = Model.Classification;
            Revision = Model.Revision;
            Ingredients = new EntryViewModel(Model.Ingredients.Clone(true));
            WorkplanId = Model.WorkplanId;
        }

        /// <summary>
        /// Copy all local information to the model to save it on the server
        /// </summary>
        protected virtual void CopyToModel()
        {
            Model.Name = _name;
            Model.Type = _type;
            Model.Classification = _classification;
            Model.Revision = _revision;
            Model.Ingredients = Ingredients.Entry;
            Model.WorkplanId = _workplanId;
        }

        /// <summary>
        /// Ingredients of this recipe model
        /// </summary>
        [Obsolete("Use property Ingredients or Model.Ingredients instead")]
        protected Entry IngredientsModel
        {
            get { return Model.Ingredients; }
            set { Model.Ingredients = value; }
        }

        /// <summary>
        /// Ingredients to use with <see cref="EntryEditor"/>
        /// </summary>
        public EntryViewModel Ingredients
        {
            get { return _ingredients; }
            private set
            {
                _ingredients = value;
                NotifyOfPropertyChange(nameof(Ingredients));
            }
        }

        /// <summary>
        /// Identifier of the recipe
        /// </summary>
        public long Id => Model.Id;

        /// <summary>
        /// Name of the recipe
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        /// <summary>
        /// Gets or sets the type of the recipe
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyOfPropertyChange(nameof(Type));
            }
        }

        /// <summary>
        /// Gets or sets the classification of the recipe
        /// </summary>
        public RecipeClassificationModel Classification
        {
            get { return _classification; }
            set
            {
                _classification = value;
                NotifyOfPropertyChange(nameof(Classification));
            }
        }

        /// <summary>
        /// Gets or sets the revision of the recipe
        /// </summary>
        public int Revision
        {
            get { return _revision; }
            set
            {
                _revision = value;
                NotifyOfPropertyChange(nameof(Revision));
            }
        }

        /// <summary>
        /// Gets or sets the corresponding workplan id
        /// </summary>
        public long WorkplanId
        {
            get { return _workplanId; }
            set
            {
                _workplanId = value;
                NotifyOfPropertyChange(nameof(WorkplanId));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="WorkplanViewModel"/> of this recipe
        /// </summary>
        public WorkplanViewModel Workplan
        {
            get { return _workplanModel; }
            set
            {
                _workplanModel = value;
                WorkplanId = _workplanModel?.Id ?? 0;
                NotifyOfPropertyChange(nameof(Workplan));
            }
        }

        /// <inheritdoc />
        public virtual void BeginEdit()
        {
        }

        /// <inheritdoc />
        public virtual void EndEdit()
        {
            CopyToModel();
        }

        /// <inheritdoc />
        public virtual void CancelEdit()
        {
            CopyFromModel();
        }
    }
}