using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.Serialization;

namespace caShared
{
    public enum RegValueType
    {
        String,
        Binary,
        DWord,
        ExpandString,
        MultiString,
        QWord,
        Unknown,
        None
    };

    [DataContract]
    [KnownType(typeof(RegStringValue))]
    [KnownType(typeof(RegBinaryValue))]
    [KnownType(typeof(RegDWORDValue))]
    [KnownType(typeof(RegQWORDValue))]
    public abstract class RegValue
    {
        [DataMember]
        public RegValueType type { get; set; }

        [DataMember]
        public String name { get; set; }
        
        public RegValue(RegValueType valType, String valName)
        {
            type = valType;
            name = valName;
        }

        public String expandStringValue { get; set; }
        public String[] multiStringValue { get; set; }
        public UInt64 qwordValue { get; set; }
    }

    [DataContract]
    public class RegStringValue : RegValue
    {
        public RegStringValue(RegValueType valType, String valName, String strVal) : base(valType, valName)
        {
            value = strVal;
        }

        [DataMember]
        public String value { get; set; }
    }

    [DataContract]
    public class RegBinaryValue : RegValue
    {
        public RegBinaryValue(RegValueType valType, String valName, byte[] binVal) : base(valType, valName)
        {
            value = binVal;
        }

        [DataMember]
        public byte[] value { get; set; }
    }

    [DataContract]
    public class RegDWORDValue : RegValue
    {
        public RegDWORDValue(RegValueType valType, String valName, UInt32 dwordVal) : base(valType, valName)
        {
            value = dwordVal;
        }

        [DataMember]
        public UInt32 value { get; set; }
    }

    [DataContract]
    public class RegQWORDValue : RegValue
    {
        public RegQWORDValue(RegValueType valType, String valName, UInt64 qwordVal) : base(valType, valName)
        {
            value = qwordVal;
        }

        [DataMember]
        public UInt64 value { get; set; }
    }
}
