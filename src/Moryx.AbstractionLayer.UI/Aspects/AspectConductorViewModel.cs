// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    /// <summary>
    /// Conductor to display aspects
    /// </summary>
    public class AspectConductorViewModel : Conductor<IScreen>.Collection.AllActive, IEditableObject
    {
        /// <summary>
        /// Text which will be displayed if no aspect is relevant
        /// </summary>
        public string EmptyText { get; }

        /// <summary>
        /// Default constructor for the conductor
        /// </summary>
        /// <param name="emptyText"> Text which will be displayed if no aspect is relevant</param>
        public AspectConductorViewModel(string emptyText)
        {
            EmptyText = emptyText;
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            base.OnActivate();

            if (Items.Any())
                ActivateItem(Items.First());
        }

        /// <inheritdoc />
        public void BeginEdit()
        {
            foreach (var aspect in Items.OfType<IEditableObject>())
                aspect.BeginEdit();
        }

        /// <inheritdoc />
        public void EndEdit()
        {
            foreach (var aspect in Items.OfType<IEditableObject>())
                aspect.EndEdit();
        }

        /// <inheritdoc />
        public void CancelEdit()
        {
            foreach (var aspect in Items.OfType<IEditableObject>())
                aspect.CancelEdit();
        }
    }
}
