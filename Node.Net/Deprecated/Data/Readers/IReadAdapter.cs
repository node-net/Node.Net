using System;
using System.IO;

namespace Node.Net.Deprecated.Data.Readers
{
    public class IReadAdapter : IRead
    {
        private object _target;
        public IReadAdapter(object value)
        {
            _target = value;
        }

        public object Read(Stream stream)
        {
            var iread = _target as IRead;
            if (iread != null) return iread.Read(stream);
            if (_target != null)
            {
                Type[] types = { typeof(Stream) };
                var readMethodInfo = _target.GetType().GetMethod("Read", types);
                if (readMethodInfo != null && readMethodInfo.ReturnType == typeof(object))
                {
                    object[] parameters = { stream };
                    return readMethodInfo.Invoke(_target, parameters);
                }
            }
            return null;
        }
    }
}
