﻿using System;

namespace Node.Net.Factories.Deprecated.Factories.Generic
{
    public class TypeFactory<T> : CompositeFactory, ITargetType, IFactory
    {
        public Type TargetType { get { return typeof(T); } }

        public override object Create(Type targetType, object source,IFactory helper)
        {
            if (TargetType.IsAssignableFrom(targetType)) return Create(source);
            return base.Create(targetType, source,helper);
        }
        // TODO : mark abstract
        public virtual T Create(object source) { return default(T); }
    }
}