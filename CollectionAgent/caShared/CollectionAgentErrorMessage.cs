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
        [DataMember]
        public String errorMessage { get; set; }

        public CollectionAgentErrorMessage(ulong ulReqID, String strErrorMsg) : base(ulReqID)
        {
            requestType = "CollectionAgentErrorMessage";
            errorMessage = strErrorMsg;
        }
    }
}
