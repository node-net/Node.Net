using System;
using System.IO;

namespace Node.Net.Deprecated.Data.Readers
{
    sealed class PrimitiveReader : IRead
    {
        public object Read(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                var header = reader.ReadLine();
                var type = reader.ReadLine();
                switch (type)
                {
                    case "System.String":
                        {
                            return reader.ReadLine();
                        }
                    case "System.Boolean":
                        {
                            return Convert.ToBoolean(reader.ReadLine());
                        }
                    case "System.Double":
                        {
                            return Convert.ToDouble(reader.ReadLine());
                        }
                }
            }
            return null;
        }

        public static PrimitiveReader Default { get; } = new PrimitiveReader();
    }
}
