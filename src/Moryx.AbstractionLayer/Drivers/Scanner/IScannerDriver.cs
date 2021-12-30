// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.AbstractionLayer (net45)'
Before:
using System;
using Moryx.AbstractionLayer.Drivers.InOut;
After:
using Moryx.AbstractionLayer.Drivers.InOut;
using System;
*/

/* Unmerged change from project 'Moryx.AbstractionLayer (net5.0)'
Before:
using System;
using Moryx.AbstractionLayer.Drivers.InOut;
After:
using Moryx.AbstractionLayer.Drivers.InOut;
using System;
*/
using System;

namespace Moryx.AbstractionLayer.Drivers.Scanner
{
    /// <summary>
    /// Common interface for barcode / QR-Code scanners
    /// </summary>
    public interface IScannerDriver : IDriver
    {
        /// <summary>
        /// Event raised when a code was read
        /// </summary>
        event EventHandler<string> CodeRead;
    }
}
