namespace NContext.Extensions.Redis
{
    using System;

    public interface ISerializer
    {
        Byte[] Serialize(Object value);

        Object Deserialize(Byte[] value);
    }
}