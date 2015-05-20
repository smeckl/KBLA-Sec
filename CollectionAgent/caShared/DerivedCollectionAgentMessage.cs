using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace caShared
{
    // This class demonstrates how to derive, serialize, and
    // deserialize message classes derived from CollectionAgentMessage
    [DataContract]
    public class DerivedCollectionAgentMessage : CollectionAgentMessage
    {
        [DataMember]
        public String derivedMessage { get; set; }

        public DerivedCollectionAgentMessage(ulong ulReqID, String strDerivedMsg) : base(ulReqID)
        {
            requestType = "DerivedCollectionAgentMessage";
            derivedMessage = strDerivedMsg;
        }
    }
}
