// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Samples (netcoreapp3.1)'
Before:
using System.ComponentModel;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/

/* Unmerged change from project 'Moryx.Products.Samples (net5.0)'
Before:
using System.ComponentModel;
using Moryx.AbstractionLayer;
After:
using Moryx.AbstractionLayer;
*/
using Moryx.AbstractionLayer.Processes;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Serialization;
using System.ComponentModel;

namespace Moryx.Products.Samples.Recipe
{
    public class WatchProductRecipe : ProductionRecipe
    {
        public WatchProductRecipe()
        {
        }

        public WatchProductRecipe(WatchProductRecipe source) : base(source)
        {
            CoresInstalled = source.CoresInstalled;

            Case = source.Case;
        }

        [EntrySerialize]
        [DisplayName("Cores Installed")]
        public int CoresInstalled { get; set; }

        [EntrySerialize]
        [DisplayName("Case Parameters")]
        public CaseDescription Case { get; set; }

        /// <inheritdoc />
        public override IRecipe Clone()
        {
            return new WatchProductRecipe(this);
        }

        /// <summary>
        /// Create a <see cref="ProductionProcess"/> for this recipe
        /// </summary>
        public override IProcess CreateProcess() =>
            new ProductionProcess { Recipe = this };
    }

    public class CaseDescription
    {
        [EntrySerialize]
        [DisplayName("Color Code")]
        [Description("Numeric code of the color")]
        public int CaseColorCode { get; set; }

        [EntrySerialize]
        [DisplayName("Material")]
        public int CaseMaterial { get; set; }
    }
}
