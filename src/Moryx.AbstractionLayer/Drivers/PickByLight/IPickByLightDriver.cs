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

namespace Moryx.AbstractionLayer.Drivers.PickByLight
{
    /// <summary>
    /// Interface for the pick by light driver
    /// </summary>
    public interface IPickByLightDriver : IDriver
    {
        /// <summary>
        /// Activate instruction for this address
        /// </summary>
        void ActivateInstruction(string address, LightInstructions instruction);

        /// <summary>
        /// Deactivate an instruction
        /// </summary>
        void DeactivateInstruction(string address);

        /// <summary>
        /// Instruction was confirmed
        /// </summary>
        event EventHandler<InstructionConfirmation> InstructionConfirmed;
    }
}
