using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace caShared
{
    [DataContract]
    public class CollectionAgentMessage
    {
        [DataMember]
        public ulong requestID { get; set; }

        [DataMember]
        public String requestType { get; set; }

        public CollectionAgentMessage() { }

        public CollectionAgentMessage(ulong ulReqID, String strReqType)
        {
            requestID = ulReqID;
            requestType = strReqType;
        }
    }
}
