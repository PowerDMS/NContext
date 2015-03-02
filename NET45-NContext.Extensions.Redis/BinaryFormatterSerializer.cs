namespace NContext.Extensions.Redis
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinaryFormatterSerializer : ISerializer
    {
        public Byte[] Serialize(Object value)
        {
            if (value == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, value);

                return memoryStream.ToArray();
            }
        }

        public Object Deserialize(Byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(value))
            {
                return binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}