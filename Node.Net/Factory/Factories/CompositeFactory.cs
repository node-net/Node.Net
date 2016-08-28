using System;
using System.Collections.Generic;

namespace Node.Net.Factory.Factories
{
    public class CompositeFactory : Dictionary<string, IFactory>, ICompositeFactory
    {
        public IFactory Parent { get; set; } = null;
        public virtual object Create(Type targetType, object source)
        {
            foreach (var name in Keys)
            {
                var childFactory = this[name] as IChild;
                if (childFactory != null) childFactory.Parent = this;

                var itargetType = this[name] as ITargetType;
                if (itargetType != null)
                {
                    if (itargetType.TargetType.IsAssignableFrom(targetType))
                    {
                        var isourceType = this[name] as ISourceType;
                        if (isourceType != null)
                        {
                            if(source == null || isourceType.SourceType.IsAssignableFrom(source.GetType()))
                            {
                                var instance = this[name].Create(targetType, source);
                                if (instance != null) return instance;
                            }
                        }
                        else
                        {

                            var instance = this[name].Create(targetType, source);
                            if (instance != null) return instance;
                        }
                    }
                }
                else
                {
                    var instance = this[name].Create(targetType, source);
                    if (instance != null) return instance;
                }
            }
            return null;
        }

        public new void Add(string key, IFactory factory)
        {
            base.Add(key, factory);
            var ichildFactory = factory as IChild;
            if (ichildFactory != null) { ichildFactory.Parent = this; }
        }

        public void Add(Node.Net.Factory.IFactoryAdapter adapter)
        {
            Add(adapter.Name, adapter);
            adapter.Parent = this;
        }
    }
}
