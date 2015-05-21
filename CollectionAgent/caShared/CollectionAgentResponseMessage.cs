using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace caShared
{
    // This class demonstrates what a query response message looks
    // like
    [DataContract]
    public class CollectionAgentResponseMessage : CollectionAgentMessage
    {
        [DataMember]
        public String responseMsg { get; set; }

        public CollectionAgentResponseMessage(ulong ulReqID, String strResponseMsg) : base(ulReqID)
        {
            requestType = "CollectionAgentResponseMessage";
            responseMsg = strResponseMsg;
        }
    }
}
