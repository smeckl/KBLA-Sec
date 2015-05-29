// Copyright 2015 Steve Meckl
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
        GetRegistryKeyRequestMessage,
        GetRegistryKeyResponseMessage,
        CollectionAgentErrorMessage
    }

    public class CollectionAgentMessageFactory
    {
        public static Dictionary<String, MessageType> MessageTypeMap = new Dictionary<string, MessageType>()
        {
            { "CollectionAgentMessage", MessageType.CollectionAgentMessage },
            { "GetRegistryKeyRequestMessage", MessageType.GetRegistryKeyRequestMessage },
            { "GetRegistryKeyResponseMessage", MessageType.GetRegistryKeyResponseMessage },
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

                case MessageType.GetRegistryKeyRequestMessage:
                    ser = new DataContractJsonSerializer(typeof(GetRegistryKeyRequestMessage));
                    caMsg = ser.ReadObject(ms) as GetRegistryKeyRequestMessage;
                    break;

                case MessageType.GetRegistryKeyResponseMessage:
                    ser = new DataContractJsonSerializer(typeof(GetRegistryKeyResponseMessage));
                    caMsg = ser.ReadObject(ms) as GetRegistryKeyResponseMessage;
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
