// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Resources.Initializer;
using System.ComponentModel;

namespace Moryx.Resources.Samples.Initializer
{
    public class SampleMachineInitializerConfig : ResourceInitializerConfig
    {
        [ReadOnly(true)]
        public override string PluginName
        {
            get { return nameof(SampleMachineInitializer); }
            set { }
        }

        [DisplayName("Machine Name"), Description("Defines the name of the machine.")]
        [DefaultValue("Sample")]
        public string MachineName { get; set; }
    }
}
