using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class RuntimeMetaData : Dictionary<WeakReference,Dictionary<string,dynamic>>
    {
        public void Clean()
        {
            List<WeakReference> to_remove = new List<WeakReference>();
            foreach(WeakReference wr in Keys)
            {
                if (!wr.IsAlive) to_remove.Add(wr);
            }
            foreach(WeakReference wr in to_remove)
            {
                Remove(wr);
            }
        }

        private WeakReference GetWeakReference(object instance)
        {
            foreach(WeakReference wr in Keys)
            {
                if (object.ReferenceEquals(wr.Target, instance)) return wr;
                if (instance.GetType().IsValueType && wr.Target.Equals(instance)) return wr;
            }
            return new WeakReference(instance);
        }
        public void Set(object instance,string key,object value)
        {
            WeakReference ref_instance = GetWeakReference(instance);
            if(!ContainsKey(ref_instance))
            {
                Add(ref_instance, new Dictionary<string, dynamic>());
            }
            this[ref_instance][key] = value;
        }

        public object Get(object instance, string key)
        {
            WeakReference ref_instance = GetWeakReference(instance);
            if(ContainsKey(ref_instance))
            {
                if(this[ref_instance].ContainsKey(key))
                {
                    return this[ref_instance][key];
                }
            }
            return null;
        }
    }
}
