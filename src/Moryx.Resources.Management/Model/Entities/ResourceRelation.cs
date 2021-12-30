// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.Model;

// ReSharper disable once CheckNamespace
namespace Moryx.Resources.Model.Entities
{
    public class ResourceRelation : EntityBase
    {
        public virtual int RelationType { get; set; }

        public virtual string SourceName { get; set; }

        public virtual long SourceId { get; set; }

        public virtual string TargetName { get; set; }

        public virtual long TargetId { get; set; }

        public virtual ResourceEntity Source { get; set; }

        public virtual ResourceEntity Target { get; set; }
    }
}
