// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.AbstractionLayer.TestTools (net45)'
Before:
using System;
After:
using Moryx.AbstractionLayer.Resources;
using System;
*/

/* Unmerged change from project 'Moryx.AbstractionLayer.TestTools (net5.0)'
Before:
using System;
After:
using Moryx.AbstractionLayer.Resources;
using System;
*/
using Moryx.AbstractionLayer.Resources;

/* Unmerged change from project 'Moryx.AbstractionLayer.TestTools (net45)'
Before:
using System.Linq;
using Moryx.AbstractionLayer.Resources;
After:
using System.Linq;
*/

/* Unmerged change from project 'Moryx.AbstractionLayer.TestTools (net5.0)'
Before:
using System.Linq;
using Moryx.AbstractionLayer.Resources;
After:
using System.Linq;
*/
using System;
using System.Collections.Generic;

namespace Moryx.AbstractionLayer.TestTools
{
    /// <summary>
    /// Dummy implementation of a resource reference collection
    /// </summary>
    public class ReferenceCollectionMock<T> : List<T>, IReferenceCollection, IReferences<T>
        where T : IResource
    {
        /// <inheritdoc />
        public ICollection<IResource> UnderlyingCollection => new List<IResource>((IEnumerable<IResource>)this);


        /// <inheritdoc />
        public event EventHandler<ReferenceCollectionChangedEventArgs> CollectionChanged;
        /// <summary>
        /// Raise the <see cref="CollectionChanged"/> event
        /// </summary>
        /// <param name="eventArgs"></param>
        public void RaiseCollectionChanged(ReferenceCollectionChangedEventArgs eventArgs)
        {
            CollectionChanged?.Invoke(this, eventArgs);
        }
    }
}
