using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace caShared
{
    [DataContract]
    public class CollectionAgentErrorMessage : CollectionAgentMessage
    {
        private static String strRequestType = "CollectionAgentErrorMessage";

        [DataMember]
        public String errorMessage { get; set; }

        public CollectionAgentErrorMessage(ulong ulReqID, String strErrorMsg) : base(ulReqID)
        {
            requestType = strRequestType;
            errorMessage = strErrorMsg;
        }

        public override bool isValid()
        {
            return (base.isValid() && 0 == requestType.CompareTo(strRequestType));
        }
    }
}
