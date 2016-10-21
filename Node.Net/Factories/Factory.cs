using System;
using System.Collections.Generic;

namespace Node.Net.Factories
{
    public class Factory : Dictionary<string,IFactory>, IFactory
    {
        public Func<Type,object,object> CreateHelperFunction { get; set; }
        public object Create(Type targetType, object source)
        {
            object item = null;
            if (CreateHelperFunction != null)
            {
                item = CreateHelperFunction(targetType, source);
                if (item != null) return item;
            }
            
            foreach(var key in Keys)
            {
                var child_factory = this[key];
                var ifactoryHelper = child_factory as IFactoryHelper;
                if (ifactoryHelper != null) ifactoryHelper.Helper = this;

                var itargetType = child_factory as ITargetType;
                if (itargetType == null || targetType.IsAssignableFrom(itargetType.TargetType))
                {
                    if (!IsLocked(key))
                    {
                        try
                        {
                            Lock(key);
                            item = child_factory.Create(targetType, source);
                        }
                        finally
                        {
                            Unlock(key);
                        }
                        if (item != null && targetType.IsAssignableFrom(item.GetType())) return item;
                    }
                }
            }
            return null;
        }

        private List<string> lockedKeys = new List<string>();
        private void Lock(string key)
        {
            lockedKeys.Add(key);
        }
        private bool IsLocked(string key)
        {
            if (lockedKeys.Contains(key)) return true;
            return false;
        }
        private void Unlock(string key)
        {
            lockedKeys.Remove(key);
        }
    }
}
