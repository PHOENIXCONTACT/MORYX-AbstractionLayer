// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0


/* Unmerged change from project 'Moryx.Products.Model (net5.0)'
Before:
using System.ComponentModel.DataAnnotations.Schema;
using Moryx.Model;
After:
using Moryx.Model;
using System.ComponentModel.DataAnnotations.Schema;
*/

/* Unmerged change from project 'Moryx.Products.Model (net45)'
Before:
using System.ComponentModel.DataAnnotations.Schema;
using Moryx.Model;
After:
using Moryx.Model;
using System.ComponentModel.DataAnnotations.Schema;
*/
using Moryx.Model;

namespace Moryx.Products.Model
{
    public class WorkplanReference : EntityBase
    {
        public virtual int ReferenceType { get; set; }

        public virtual long SourceId { get; set; }

        public virtual long TargetId { get; set; }

        public virtual WorkplanEntity Target { get; set; }

        public virtual WorkplanEntity Source { get; set; }
    }
}
