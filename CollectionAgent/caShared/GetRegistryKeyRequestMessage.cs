using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace caShared
{
    public enum RootKey
    {
        HKEY_CLASSES_ROOT,
        HKEY_CURRENT_USER,
        HKEY_LOCAL_MACHINE,
        HKEY_USERS,
        HKEY_CURRENT_CONFIG
    };

    // This class demonstrates how to derive, serialize, and
    // deserialize message classes derived from CollectionAgentMessage
    [DataContract]
    public class GetRegistryKeyRequestMessage : CollectionAgentMessage
    {
        private static String strRequestType = "GetRegistryKeyRequestMessage";
        private static String keyPathRegEx = "^" + CollectionAgentErrorMessage.RegexPrintableChars + "*$";

        [DataMember]
        public RootKey root { get; set; }

        [DataMember]
        public String keyPath { get; set; }

        public GetRegistryKeyRequestMessage(ulong ulReqID, RootKey rootKey, String strKeyPath) : base(ulReqID)
        {
            requestType = strRequestType;
            root = rootKey;
            keyPath = strKeyPath;
        }

        public override bool isValid()
        {
            bool retVal = true;

            Regex pathRegEx = new Regex(keyPathRegEx, RegexOptions.IgnoreCase);

            if (!pathRegEx.IsMatch(keyPath))
                retVal = false;

            return (base.isValid() && 0 == requestType.CompareTo(strRequestType) && retVal);
        }
    }
}
