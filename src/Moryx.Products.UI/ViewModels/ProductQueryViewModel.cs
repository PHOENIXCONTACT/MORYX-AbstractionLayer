// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Moryx.Products.UI.ProductService;

namespace Moryx.Products.UI
{
    public class ProductQueryViewModel : PropertyChangedBase
    {
        private RevisionFilter _revisionFilter;
        private Selector _selector;
        private RecipeFilter _recipeFilter;
        private string _identifier;
        private short _revision;
        private string _name;
        private ProductDefinitionViewModel _type;
        private List<PropertyFilterViewModel> _propertyFilters;

        public RevisionFilter RevisionFilter
        {
            get => _revisionFilter;
            set
            {
                _revisionFilter = value;
                NotifyOfPropertyChange();
            }
        }

        public Selector Selector
        {
            get => _selector;
            set
            {
                _selector = value;
                NotifyOfPropertyChange();
            }
        }

        public RecipeFilter RecipeFilter
        {
            get => _recipeFilter;
            set
            {
                _recipeFilter = value;
                NotifyOfPropertyChange();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                NotifyOfPropertyChange();
            }
        }

        public short Revision
        {
            get => _revision;
            set
            {
                _revision = value;
                NotifyOfPropertyChange();
            }
        }

        public ProductDefinitionViewModel Type
        {
            get => _type;
            set
            {
                // Clear property filters if type was changed
                if (value == null || (_type != null && value.Model.Name != _type.Model.Name))
                    PropertyFilters = null;

                _type = value;
                NotifyOfPropertyChange();
            }
        }

        public List<PropertyFilterViewModel> PropertyFilters
        {
            get => _propertyFilters;
            set
            {
                _propertyFilters = value;
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
                Revision = Revision,
                Type = Type?.Model.Name,
            };

            if (PropertyFilters != null)
            {
                query.PropertyFilters = PropertyFilters.Select(pf => new PropertyFilter
                {
                    Entry = pf.Property.ToServiceEntry(),
                    Operator = pf.Operator
                }).ToArray();
            }

            if (!string.IsNullOrWhiteSpace(Identifier))
                query.Identifier = Identifier;

            if (!string.IsNullOrWhiteSpace(Name))
                query.Name = Name;

            return query;
        }
    }
}
