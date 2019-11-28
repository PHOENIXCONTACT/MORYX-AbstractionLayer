using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    [DataContract]
    public class TypedAspectConfiguration
    {
        public TypedAspectConfiguration()
        {
            Aspects = new List<AspectConfiguration>();
        }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public List<AspectConfiguration> Aspects { get; set; }
    }
}