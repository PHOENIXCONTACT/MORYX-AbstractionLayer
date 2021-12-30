// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer.Activities;
using Moryx.AbstractionLayer.Capabilities;
using Moryx.AbstractionLayer.Processes;
using Moryx.AbstractionLayer.Tasks;
using Moryx.Serialization;

namespace Moryx.Products.IntegrationTests.RecopeStorage
{
    public class TaskB : TaskStep<ActivityB, ParametersB>
    {
        public override string Name => nameof(TaskB);
    }

    public class ParametersB : Parameters
    {
        [EntrySerialize]
        public SubParameter[] Subs { get; set; }

        protected override void Populate(IProcess process, Parameters instance)
        {
            var parameters = (ParametersB)instance;
            parameters.Subs = Subs;
        }
    }

    public class SubParameter
    {
        public int Type { get; set; }

        public string Name { get; set; }
    }

    [ActivityResults(typeof(DefaultActivityResult))]
    public class ActivityB : Activity<ParametersB>
    {
        public override ProcessRequirement ProcessRequirement { get; }

        public override ICapabilities RequiredCapabilities { get; }

        protected override ActivityResult CreateResult(long resultNumber)
        {
            throw new System.NotImplementedException();
        }

        protected override ActivityResult CreateFailureResult()
        {
            throw new System.NotImplementedException();
        }
    }
}
