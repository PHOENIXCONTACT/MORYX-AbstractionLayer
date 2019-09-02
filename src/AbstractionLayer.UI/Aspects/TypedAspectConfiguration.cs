using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    [DataContract]
    public class TypedAspectConfiguration
    {
        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public List<AspectConfiguration> Aspects { get; set; }
    }
}