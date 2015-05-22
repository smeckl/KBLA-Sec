using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace caShared
{    
    [DataContract]
    [KnownType(typeof(DerivedCollectionAgentMessage))]
    public class CollectionAgentMessage
    {
        private static String strRequestType = "CollectionAgentMessage";

        [DataMember]
        public ulong requestID { get; set; }

        [DataMember]
        public String requestType { get; set; }

        public CollectionAgentMessage() { }

        public CollectionAgentMessage(ulong ulReqID)
        {
            requestID = ulReqID;
            requestType = strRequestType;
        }

        public String ToJSON()
        {
            //Create a stream to serialize the object to.
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(this.GetType());
            ser.WriteObject(ms, this);

            byte[] jsonMsg = ms.ToArray();
            ms.Close();

            // Use Decoder class to convert from bytes to UTF8 
            // in case a character spans two buffers.
            Decoder decoder = Encoding.UTF8.GetDecoder();

            int bytes = jsonMsg.Length;
            char[] chars = new char[decoder.GetCharCount(jsonMsg, 0, bytes)];
            decoder.GetChars(jsonMsg, 0, bytes, chars, 0);

            StringBuilder strJSONMsg = new StringBuilder();
            strJSONMsg.Append(chars);
            strJSONMsg.Append("<EOF>");

            return strJSONMsg.ToString();
        }   
        
        public virtual Boolean isValid()
        {
            return (requestID > 0);
        }     
    }
}
