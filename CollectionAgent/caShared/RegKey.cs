using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace caShared
{
    [DataContract]
    public class RegKey
    {
        [DataMember]
        public String path { get; set; }

        [DataMember]
        public RegKey[] subKeys { get; set; }
        [DataMember]
        public RegValue[] values { get; set; }
    }
}
