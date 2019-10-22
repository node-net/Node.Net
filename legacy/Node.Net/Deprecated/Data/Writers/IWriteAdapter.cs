using System;
using System.IO;

namespace Node.Net.Deprecated.Data.Writers
{
    public class IWriteAdapter : IWrite
    {
        private object _target;
        public IWriteAdapter(object value)
        {
            _target = value;
        }

        public void Write(Stream stream, object value)
        {
            var iwrite = _target as IWrite;
            if (iwrite != null)
            {
                iwrite.Write(stream, value);
            }
            else
            {
                if (_target != null)
                {
                    Type[] types = { typeof(Stream), typeof(object) };
                    var writeMethodInfo = _target.GetType().GetMethod(nameof(Write), types);
                    if (writeMethodInfo != null)
                    {
                        object[] parameters = { stream, value };
                        writeMethodInfo.Invoke(_target, parameters);
                    }
                }
            }

        }
    }
}
