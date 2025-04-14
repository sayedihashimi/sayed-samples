using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecurityScanner.Razor.Samples;
public class InsecureDeserializationExample
{
    // SECURITY ISSUE: Insecure deserialization
    public object Deserialize(byte[] data)
    {
// pragma is required, otherwise the line of code bellow will show an error
#pragma warning disable SYSLIB0011
        
        var formatter = new BinaryFormatter();
        return formatter.Deserialize(new MemoryStream(data));
#pragma warning restore SYSLIB0011
    }
}
