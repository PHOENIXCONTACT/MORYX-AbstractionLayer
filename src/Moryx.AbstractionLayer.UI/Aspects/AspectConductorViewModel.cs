// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Moryx.AbstractionLayer.UI.Aspects
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
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (Items.Any())
                await ActivateItemAsync(Items.First(), cancellationToken);

            await base.OnActivateAsync(cancellationToken);
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
