using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Behesht.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteArray(this object value)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, value);
            return ms.ToArray();
        }

        public static TItem ToObject<TItem>(this byte[] cachedBytes)
        {
            using var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(cachedBytes, 0, cachedBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (TItem)binForm.Deserialize(memStream);
            return obj;
        }
    }
}
