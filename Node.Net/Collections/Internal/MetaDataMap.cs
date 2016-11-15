using System;
using System.Collections.Generic;

namespace Node.Net.Collections.Internal
{
    sealed class MetaDataMap : Dictionary<WeakReference, Dictionary<string, dynamic>>
    {
        public Dictionary<string, dynamic> GetMetaData(object item)
        {
            if (Count > 1000) Clean();
            foreach (var wr in Keys)
            {
                if (wr.Target != null)
                {
                    /*
                    if (wr.Target.GetType().IsValueType)
                    {
                        if (wr.Target.Equals(item)) return this[wr];
                    }
                    else
                    {
                        if (wr.Target == item) return this[wr];
                    }*/
                    if (wr.Target == item) return this[wr];
                }
            }
            var metaData = new Dictionary<string, dynamic>();
            Add(new WeakReference(item), metaData);
            return metaData;
        }
        public static MetaDataMap Default { get; } = new MetaDataMap();

        //public static Func<object, Dictionary<string, dynamic>> GetMetaDataFunction = Default.GetMetaData;

        public void Clean()
        {
            var deadKeys = new List<WeakReference>();
            foreach (var wr in Keys)
            {
                if (!wr.IsAlive) deadKeys.Add(wr);
            }
            foreach (var deadKey in deadKeys) { Remove(deadKey); }
        }
    }
}
