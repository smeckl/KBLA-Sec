using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace caShared
{
    public enum MessageType
    {
        CollectionAgentMessage,
        DerivedCollectionAgentMessage,
        CollectionAgentResponseMessage,
        CollectionAgentErrorMessage
    }

    public class CollectionAgentMessageFactory
    {
        public static Dictionary<String, MessageType> MessageTypeMap = new Dictionary<string, MessageType>()
        {
            { "CollectionAgentMessage", MessageType.CollectionAgentMessage },
            { "DerivedCollectionAgentMessage", MessageType.DerivedCollectionAgentMessage },
            { "CollectionAgentResponseMessage", MessageType.CollectionAgentResponseMessage },
            { "CollectionAgentErrorMessage", MessageType.CollectionAgentErrorMessage }
        };

        public static CollectionAgentMessage constructMessageFromJSON(String strJSON)
        {
            // If there is a trailing <EOF> character, strip it so that JSON
            // deserialization will work correctly
            int index = (strJSON.IndexOf("<EOF>"));
            if (index != -1)
            {
                strJSON = strJSON.Substring(0, index);
            }

            // Create a new CollectionAgentMsg object to serialize to
            CollectionAgentMessage deserializedMsg = new CollectionAgentMessage();

            // Read String data into a MemoryStream so it can be deserialized
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJSON));

            // Deserialize the stream into an object
            // TODO: May need to handle exceptions here.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedMsg.GetType());
            deserializedMsg = ser.ReadObject(ms) as CollectionAgentMessage;
            ms.Close();

            return buildDerivedMessageObject(strJSON, deserializedMsg.requestType);
        }

        private static CollectionAgentMessage buildDerivedMessageObject(String strMessage, String strRequestType)
        {
            // Use the string value to get the message type
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

                case MessageType.CollectionAgentResponseMessage:
                    ser = new DataContractJsonSerializer(typeof(CollectionAgentResponseMessage));
                    caMsg = ser.ReadObject(ms) as CollectionAgentResponseMessage;
                    break;

                case MessageType.CollectionAgentErrorMessage:
                    ser = new DataContractJsonSerializer(typeof(CollectionAgentErrorMessage));
                    caMsg = ser.ReadObject(ms) as CollectionAgentErrorMessage;
                    break;

                default:
                    ser = null;
                    break;
            }

            ms.Close();

            return caMsg.isValid() ? caMsg : null;
        }
    }
}
