using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using caShared;

namespace CollectionAgent
{
    class GetRegistryKeyRequestProcessor
    {
        private GetRegistryKeyRequestMessage requestMessage { get; set; }

        public GetRegistryKeyRequestProcessor(GetRegistryKeyRequestMessage requestMsg)
        {
            requestMessage = requestMsg;
        }

        public CollectionAgentMessage processRequest()
        {
            CollectionAgentMessage responseMsg = null;

            RegistryKey regKey = null;

            // Open the key from the appropriate root key
            switch(requestMessage.root)
            {
                case RootKey.HKEY_CLASSES_ROOT:
                    regKey = Registry.ClassesRoot.OpenSubKey(requestMessage.keyPath, false);
                    break;

                case RootKey.HKEY_CURRENT_CONFIG:
                    regKey = Registry.CurrentConfig.OpenSubKey(requestMessage.keyPath, false);
                    break;

                case RootKey.HKEY_CURRENT_USER:
                    regKey = Registry.CurrentUser.OpenSubKey(requestMessage.keyPath, false);
                    break;

                case RootKey.HKEY_LOCAL_MACHINE:
                    regKey = Registry.LocalMachine.OpenSubKey(requestMessage.keyPath, false);
                    break;

                case RootKey.HKEY_USERS:
                    regKey = Registry.Users.OpenSubKey(requestMessage.keyPath, false);
                    break;
            }

            // If we found the key, then read the values and 
            if (null != regKey)
            {
                GetRegistryKeyResponseMessage regResponse = new GetRegistryKeyResponseMessage(requestMessage.requestID);

                // Set the path on the Registry key
                regResponse.regKey.path = requestMessage.keyPath;

                populateRegistrykey(regResponse.regKey, regKey);

                responseMsg = regResponse;
            }
            else // send an error message instead
            {
                responseMsg = new CollectionAgentErrorMessage(requestMessage.requestID, "Registry key not found");
            }

            return responseMsg;
        }

        private void populateRegistrykey(caShared.RegKey respKey, RegistryKey sysKey)
        {
            // First, populate the values for this key
            populateRegKeyValues(respKey, sysKey);

            // Second, recursively populate sub-keys for this key
            String[] subKeyNames = sysKey.GetSubKeyNames();

            if (subKeyNames.Length > 0)
            {
                RegKey[] childKeys = new RegKey[subKeyNames.Length];

                for (int i = 0; i < subKeyNames.Length; i++)
                {
                    RegistryKey subKey = sysKey.OpenSubKey(subKeyNames[i], false);

                    // Allocate a new RegKey to hold the subkey
                    childKeys[i] = new RegKey();

                    // Populate the subkey
                    populateRegistrykey(childKeys[i], subKey);
                }

                respKey.subKeys = childKeys;
            }
        }

        private void populateRegKeyValues(caShared.RegKey respKey, RegistryKey sysKey)
        {
            String[] valNames = sysKey.GetValueNames();

            if (valNames.Length > 0)
            {
                RegValue[] values = new RegValue[valNames.Length];

                for (int i = 0; i < valNames.Length; i++)
                {
                    RegistryValueKind valKind = sysKey.GetValueKind(valNames[0]);

                    switch (valKind)
                    {
                        case RegistryValueKind.String:
                            RegValue val = new RegStringValue(RegValueType.String, valNames[i], (String)sysKey.GetValue(valNames[i]));
                            values[i] = (RegValue)val;
                            break;

                        case RegistryValueKind.Binary:

                            break;

                        case RegistryValueKind.DWord:

                            break;

                        case RegistryValueKind.ExpandString:

                            break;

                        case RegistryValueKind.MultiString:

                            break;

                        case RegistryValueKind.QWord:

                            break;

                        case RegistryValueKind.Unknown:

                            break;

                        case RegistryValueKind.None:

                            break;
                    }                    
                }

                respKey.values = values;
            }
        }
    }
}
