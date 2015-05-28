using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization;

namespace caShared
{
    // This class demonstrates what a query response message looks
    // like
    [DataContract]
    public class GetRegistryKeyResponseMessage : CollectionAgentMessage
    {
        private static String strRequestType = "GetRegistryKeyResponseMessage";

        public GetRegistryKeyResponseMessage(ulong ulReqID) : base(ulReqID)
        {
            requestType = "GetRegistryKeyResponseMessage";
            regKey = new RegKey();
        }

        [DataMember]
        public RegKey regKey { get; set; }

        public override bool isValid()
        {                 
            return (base.isValid() && 0 == requestType.CompareTo(strRequestType));
        }
    }
}
