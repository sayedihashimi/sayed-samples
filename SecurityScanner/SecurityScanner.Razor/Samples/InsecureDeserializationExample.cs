using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecurityScanner.Razor;
public class InsecureDeserializationExample
{
    // SECURITY ISSUE: Insecure deserialization
    public object Deserialize(byte[] data)
    {
#pragma warning disable SYSLIB0011
        var formatter = new BinaryFormatter();
        return formatter.Deserialize(new MemoryStream(data));
#pragma warning restore SYSLIB0011
    }
}
