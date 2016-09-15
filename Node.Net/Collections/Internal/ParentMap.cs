using System;
using System.Collections.Generic;

namespace Node.Net.Collections.Internal
{
    class ParentMap : Dictionary<WeakReference, WeakReference>
    {
        public void Clean()
        {
            var deadKeys = new List<WeakReference>();
            foreach (var wr in Keys)
            {
                if (!wr.IsAlive) deadKeys.Add(wr);
                else
                {
                    if (this[wr] == null || !this[wr].IsAlive) deadKeys.Add(wr);
                }
            }
            foreach (var deadKey in deadKeys) { Remove(deadKey); }
        }

        public object GetParent(object source)
        {
            foreach (var wr in Keys)
            {
                if (wr.Target == source)
                {
                    return this[wr].Target;
                }
            }
            return null;
        }

        public void SetParent(object source, object parent)
        {
            var deadKeys = new List<WeakReference>();

            foreach (var wr in Keys)
            {
                if (object.ReferenceEquals(wr.Target, source))
                {
                    deadKeys.Add(wr);
                }
            }

            Add(new WeakReference(source), new WeakReference(parent));
            foreach (var deadKey in deadKeys) { Remove(deadKey); }
        }
    }
}