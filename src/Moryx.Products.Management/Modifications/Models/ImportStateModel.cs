// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Products.Management.Implementations;
using System.Runtime.Serialization;

namespace Moryx.Products.Management.Modification.Models
{
    [DataContract]
    internal class ImportStateModel
    {
        public ImportStateModel()
        {
        }

        public ImportStateModel(ImportState state)
        {
            Session = state.Session.ToString();
            Completed = state.Completed;
            ErrorMessage = state.ErrorMsg;
        }

        [DataMember]
        public string Session { get; set; }

        [DataMember]
        public bool Completed { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}