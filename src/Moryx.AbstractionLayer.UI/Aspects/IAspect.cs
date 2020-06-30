// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Moryx.AbstractionLayer.UI.Aspects
{
    /// <summary>
    /// Interface for aspects
    /// </summary>
    public interface IAspect : IScreen, IEditableObject
    {
        /// <summary>
        /// Indicates if the edit mode is active or not
        /// </summary>
        bool IsEditMode { get; }

        /// <summary>
        /// Validates the aspect before saving
        /// </summary>
        void Validate(ICollection<ValidationResult> validationErrors);

        /// <summary>
        /// Saves the current changes of the aspect
        /// </summary>
        Task Save();
    }

    /// <summary>
    /// Interface for aspects
    /// </summary>
    public interface IAspect<in T> : IAspect
    {
        /// <summary>
        /// Indicates if this aspect is currently relevant
        /// </summary>
        bool IsRelevant(T editableObject);

        /// <summary>
        /// Loads additional data of an aspect by the given object
        /// </summary>
        Task Load(T editableObject);
    }
}
