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
    public enum MessageType
    {
        CollectionAgentMessage,
        DerivedCollectionAgentMessage
    }

    [DataContract]
    [KnownType(typeof(DerivedCollectionAgentMessage))]
    public class CollectionAgentMessage
    {
        public static Dictionary<String, caShared.MessageType> MessageTypeMap = new Dictionary<string, caShared.MessageType>()
        {
            { "CollectionAgentMessage", caShared.MessageType.CollectionAgentMessage },
            { "DerivedCollectionAgentMessage", caShared.MessageType.DerivedCollectionAgentMessage }
        };

        [DataMember]
        public ulong requestID { get; set; }

        [DataMember]
        public String requestType { get; set; }

        public CollectionAgentMessage() { }

        public CollectionAgentMessage(ulong ulReqID)
        {
            requestID = ulReqID;
            requestType = "CollectionAgentMessage";
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

        public static CollectionAgentMessage deserializeMessage(String strMessage)
        {
            // If there is a trailing <EOF> character, strip it so that JSON
            // deserialization will work correctly
            int index = (strMessage.IndexOf("<EOF>"));
            if (index != -1)
            {
                strMessage = strMessage.Substring(0, index);
            }

            // Create a new CollectionAgentMsg object to serialize to
            CollectionAgentMessage deserializedMsg = new CollectionAgentMessage();

            // Read String data into a MemoryStream so it can be deserialized
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strMessage));

            // Deserialize the stream into an object
            // TODO: May need to handle exceptions here.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedMsg.GetType());
            deserializedMsg = ser.ReadObject(ms) as CollectionAgentMessage;
            ms.Close();

            return buildDerivedMessageObject(strMessage, deserializedMsg.requestType);
        }

        private static CollectionAgentMessage buildDerivedMessageObject(String strMessage, String strRequestType)
        {
            MessageType msgType = MessageTypeMap[strRequestType];

            CollectionAgentMessage caMsg = null;

            // Read String data into a MemoryStream so it can be deserialized
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strMessage));

            // Deserialize the stream into an object
            // TODO: May need to handle exceptions here.
            DataContractJsonSerializer ser = null;

            switch (msgType)
            {
                case MessageType.CollectionAgentMessage:
                    ser = new DataContractJsonSerializer(typeof(CollectionAgentMessage));
                    caMsg = ser.ReadObject(ms) as CollectionAgentMessage;
                    break;

                case MessageType.DerivedCollectionAgentMessage:
                    ser = new DataContractJsonSerializer(typeof(DerivedCollectionAgentMessage));
                    caMsg = ser.ReadObject(ms) as DerivedCollectionAgentMessage;
                    break;

                default:
                    ser = null;
                    break;
            }

            ms.Close();

            return caMsg;
        }
    }
}
