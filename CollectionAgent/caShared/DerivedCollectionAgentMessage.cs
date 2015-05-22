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
        private static String strRequestType = "DerivedCollectionAgentMessage";

        [DataMember]
        public String derivedMessage { get; set; }

        public DerivedCollectionAgentMessage(ulong ulReqID, String strDerivedMsg) : base(ulReqID)
        {
            requestType = strRequestType;
            derivedMessage = strDerivedMsg;
        }

        public override bool isValid()
        {
            return (base.isValid() && 0 == requestType.CompareTo(strRequestType));
        }
    }
}
