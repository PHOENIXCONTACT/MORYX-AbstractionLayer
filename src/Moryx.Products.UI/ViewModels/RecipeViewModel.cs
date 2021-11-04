// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.ComponentModel;
using Caliburn.Micro;
using Moryx.AbstractionLayer.UI;
using Moryx.Controls;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI
{
    /// <summary>
    /// ViewModel for recipes of a product
    /// </summary>
    public class RecipeViewModel : PropertyChangedBase, IEditableObject, IIdentifiableObject
    {
        private string _name;
        private string _type;
        private int _revision;
        private RecipeClassificationModel _classification;

        private WorkplanViewModel _workplan;
        private EntryViewModel _properties;

        /// <summary>
        /// Underlying model
        /// </summary>
        public RecipeModel Model { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeViewModel"/> class.
        /// </summary>
        public RecipeViewModel(RecipeModel recipeModel)
        {
            Model = recipeModel;
            CopyFromModel();
        }

        /// <summary>
        /// Updates the internal model and raises NotifyPropertyChanged
        /// </summary>
        /// <param name="recipeModel">The updated model instance</param>
        public void UpdateModel(RecipeModel recipeModel)
        {
            Model = recipeModel;
            CopyFromModel();
        }

        /// <summary>
        /// Additional properties of this recipe
        /// </summary>
        public EntryViewModel Properties
        {
            get { return _properties; }
            private set
            {
                _properties = value;
                NotifyOfPropertyChange(nameof(Properties));
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
        /// Gets or sets the <see cref="WorkplanViewModel"/> of this recipe
        /// </summary>
        public WorkplanViewModel Workplan
        {
            get { return _workplan; }
            set
            {
                _workplan = value;
                NotifyOfPropertyChange(nameof(Workplan));
            }
        }

        /// <summary>
        /// Copy all model information to the defined view model properties
        /// </summary>
        protected void CopyFromModel()
        {
            Name = Model.Name;
            Type = Model.Type;
            Classification = Model.Classification;
            Revision = Model.Revision;
            Properties = new EntryViewModel(Model.Properties.ToSerializationEntry());
        }

        /// <summary>
        /// Copy all local information to the model to save it on the server
        /// </summary>
        protected void CopyToModel()
        {
            Model.Name = _name;
            Model.Type = _type;
            Model.Classification = _classification;
            Model.Revision = _revision;
            Model.Properties = Properties.Entry.ToServiceEntry();
            Model.WorkplanId = Workplan?.Id ?? 0;
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
