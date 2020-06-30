// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI.Interaction
{
    internal class ProductQueryViewModel : PropertyChangedBase
    {
        private RevisionFilter _revisionFilter;
        private Selector _selector;
        private RecipeFilter _recipeFilter;
        private string _identifier;
        private short _revision;
        private string _name;

        public RevisionFilter RevisionFilter
        {
            get { return _revisionFilter; }
            set
            {
                _revisionFilter = value;
                NotifyOfPropertyChange();
            }
        }

        public Selector Selector
        {
            get { return _selector; }
            set
            {
                _selector = value;
                NotifyOfPropertyChange();
            }
        }

        public RecipeFilter RecipeFilter
        {
            get { return _recipeFilter; }
            set
            {
                _recipeFilter = value;
                NotifyOfPropertyChange();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Identifier
        {
            get { return _identifier; }
            set
            {
                _identifier = value;
                NotifyOfPropertyChange();
            }
        }

        public short Revision
        {
            get { return _revision; }
            set
            {
                _revision = value;
                NotifyOfPropertyChange();
            }
        }

        public ProductQuery GetQuery()
        {
            var query = new ProductQuery
            {
                RevisionFilter = RevisionFilter,
                RecipeFilter = RecipeFilter,
                Selector = Selector,
                Revision = Revision
            };

            if (!string.IsNullOrWhiteSpace(Identifier))
                query.Identifier = Identifier;

            if (!string.IsNullOrWhiteSpace(Name))
                query.Name = Name;

            return query;
        }
    }
}
